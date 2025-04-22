using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using backend.Data;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
	ContentRootPath = Directory.GetCurrentDirectory(),
	WebRootPath = ""
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

builder.Services.AddDbContext<AppDbContext>(options => 
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Optional: Enable Swagger in development
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();  // For NSwag or use app.UseSwagger(); with Swashbuckle
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

// Redirect to '/' for any non-matching routes
app.MapFallback(async context =>
{
    context.Response.Redirect("/", permanent: false);  // Redirect to "/"
    await Task.CompletedTask;
});



app.Run();
