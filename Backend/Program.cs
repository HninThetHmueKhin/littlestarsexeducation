using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Serve static files from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run();
