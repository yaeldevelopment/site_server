

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://site-ahuu.onrender.com") // Specify allowed origins
                     .AllowAnyMethod() // Allow all HTTP methods (GET, POST, PUT, DELETE, etc.)
                     .AllowAnyHeader(); // Allow all headers
    });
});
// ����� ������ Swagger
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api", Version = "v1" });
            });
var app = builder.Build();

// Middleware �-Swagger
if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "api v1");
                   c.RoutePrefix = "swagger"; // תוכל לשנות את זה אם תרצה נתיב אחר
            });
}

     


// Enable CORS
app.UseCors("AllowSpecificOrigin");

// Other middleware (like routing, authorization, etc.)
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseAuthorization();
app.MapControllers();
app.Run();
//app.Run("http://0.0.0.0:8080"); // Ensure the app listens on 8080

