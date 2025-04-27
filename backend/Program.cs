using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using backend.Data;
using backend.Models;
using backend.Services;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	ContentRootPath = Directory.GetCurrentDirectory(),
	WebRootPath = ""
});

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();


// connect to db
builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<PasswordService>();

var app = builder.Build();

// Optional: Enable Swagger in development
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseRouting();

// Serve static frontend (e.g., Svelte) files
app.UseStaticFiles(new StaticFileOptions
{
	FileProvider = new PhysicalFileProvider(
		Path.Combine(Directory.GetCurrentDirectory(), "..", "frontend", "public")),
	RequestPath = ""  // Serve at root
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

app.MapPost("/api/user/login", async (User user, UserService userService, PasswordService passwordService) => {
	User? userT = null;

	user.PasswordHash = passwordService.HashPassword(user.PasswordHash!);

	try
	{
        userT = await userService.LoginAsync(user);
	}
	catch (ArgumentException e)
	{
		Results.NotFound(e.Message);
	}
	catch (UnauthorizedAccessException)
	{
		return Results.Unauthorized();
	}

    return Results.Ok(userT);
});

app.MapPost("/api/user/signup", async (User user, UserService userService) =>
{
	try
	{
		await userService.AddUserAsync(user);
	}
	catch (Exception e)
	{
		return Results.Conflict(e.Message);
	}

	return Results.Ok(user);
});

// Redirect to '/' for any non-matching routes
app.MapFallback(async context =>
{

	context.Response.Redirect("/", permanent: false);  // Redirect to "/"
	await Task.CompletedTask;
});



app.Run();
