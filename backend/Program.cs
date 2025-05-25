using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using backend.Data;
using backend.Models;
using backend.Services;
using backend.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;



var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	ContentRootPath = Directory.GetCurrentDirectory(),
	WebRootPath = ""
});



builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
	var jwtSettings = builder.Configuration.GetSection("Jwt");
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateAudience = true,
		ValidateLifetime = false,
		ValidateIssuerSigningKey = true,
		ValidIssuer = jwtSettings["Issuer"],
		ValidAudience = jwtSettings["Audience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
	};
});

builder.Services.AddAuthorization();

// connect to db
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<JwtService>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}



app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(Directory.GetCurrentDirectory(), "..", "frontend", "public")),
	RequestPath = ""
});

app.MapWhen(context => context.Request.Path == "/", builder =>
{
	builder.Run(async context =>
	{
		var file = Path.Combine(Directory.GetCurrentDirectory(), "..", "frontend", "public", "index.html");
		context.Response.ContentType = "text/html";
		await context.Response.SendFileAsync(file);
	});
});

// app.MapGet("/{username}", async (string username, AppDbContext db) => 
// {
// 	Console.WriteLine($"Requested username: {username}");
// 	var user = await db.Users.Where(u => u.Name == username ).FirstOrDefaultAsync();

// 	if (user is null)
// 	{
// 		return Results.NotFound();
// 	}

// 	return Results.Ok(user);
// });




app.MapGet("/api/user/{username}", async (string username, AppDbContext db) =>
{
	Console.WriteLine($"Requested username: {username}");
	var user = await db.Users.Where(u => u.Name == username).FirstOrDefaultAsync();

	if (user is null)
	{
		return Results.NotFound();
	}

	return Results.Ok(user);
});

app.MapPost("/api/user/login", async (LoginRequest loginR, UserService userService, PasswordService passwordService, JwtService jwtService) =>
{
	RegisteredUser user;

	try
	{
		user = await userService.LoginAsync(loginR);
	}
	catch (ArgumentException e)
	{
		return Results.Json(new { error = e.Message }, statusCode: 400);
	}
	catch (UnauthorizedAccessException e)
	{
		return Results.Json(new { error = e.Message }, statusCode: 401);
	}

	var token = jwtService.GenerateToken(user);

	return Results.Ok(new { token });
});

app.MapPost("/api/user/guest", async (SignupRequestGuest signupR, UserService userService, JwtService jwtService) =>
{
	GuestUser guest = await userService.AddUserAsync(signupR);
	var token = jwtService.GenerateToken(guest);
	return Results.Ok(new { token });
});

app.MapPost("/api/user/signup", async (SignupRequest signupR, UserService userService, JwtService jwtService) =>
{
	RegisteredUser user;
	try
	{
		user = await userService.AddUserAsync(signupR);
	}
	catch (Exception e)
	{
		return Results.Conflict(e.Message);
	}

	var token = jwtService.GenerateToken(user);
	return Results.Ok(new { token });
});


app.MapPost("/api/user/edit", async (User user, UserService userService) =>
{
	try
	{
		await userService.EditUserAsync(user);
	}
	catch (Exception e)
	{
		return Results.Conflict(e.Message);
	}

	return Results.Ok(user);
}).RequireAuthorization();

app.MapPost("/api/user/delete", async (User user, UserService userService) =>
{
	try
	{
		await userService.DeleteUserAsync(user);
	}
	catch (Exception e)
	{
		return Results.Conflict(e.Message);
	}

	return Results.Ok(user);
}).RequireAuthorization();

app.MapGet("/api/user/get-plans/{userId?}", async (int? userId, HttpContext http, UserService userService) =>
{
	if (!userId.HasValue)
	{
		var userIdStr = http.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		
		if (userIdStr is null) return Results.Ok();
		userId = int.Parse(userIdStr);
	}

	var user = await userService.GetUserAsync(userId.Value);
	if (user is null) return Results.Ok();

	var plans = await userService.GetUserPlansAsync(user);
	return Results.Ok(plans);
});

app.MapPost("/api/plan/get-users", async (Plan plan, PlanService planService) =>
{
	var users = await planService.GetPlanUsersAsync(plan);
	return Results.Ok(users);
});



app.MapPost("/api/plan/create", async (HttpContext ctx, PlanRequest planR, PlanService planService, AppDbContext db) =>
{
	Plan plan;

	try
	{
		plan = await planService.CreatePlanAsync(planR);
	}
	catch (Exception e)
	{
		return Results.Conflict(e.Message);
	}


	var planUser = new PlanUser
	{
		UserId = int.Parse(ctx.User.FindFirst(ClaimTypes.NameIdentifier)!.Value),
		PlanId = plan.Id,
		Role = Role.Creator
	};

	await db.PlanUsers.AddAsync(planUser);
	await db.SaveChangesAsync();

	return Results.Ok(plan);

}).RequireAuthorization();

app.MapPost("/api/plan/edit", async (Plan plan, PlanService planService) =>
{
	await planService.EditPlanAsync(plan);
	return Results.Ok(plan);
}).RequireAuthorization();

app.MapPost("/api/plan/delete", async (Plan plan, PlanService planService) =>
{
	await planService.DeletePlanAsync(plan);
	return Results.Ok();
}).RequireAuthorization();


app.MapFallback(async (context) =>
{
	context.Response.Redirect("/", permanent: false);
	await Task.CompletedTask;
});



app.Run();
