using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Services;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add controllers to the container.
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonTypeService, PersonTypeService>();

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

// Add versioned API explorer (needed for Swagger to support versions)
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // e.g. v1, v2
    options.SubstituteApiVersionInUrl = true;
});

// Add versioned API explorer (needed for Swagger to support versions)
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // e.g. v1, v2
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddDbContext<PersonAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var allowSpeficOrigins = "_allowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowSpeficOrigins,
        policy =>
        {
            // Allow specific origins
            policy.WithOrigins(
                "http://localhost:8080",
                "http://localhost:3000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PersonAPIContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"Person API {description.ApiVersion}");
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.UseCors(allowSpeficOrigins);
app.Run();

public partial class Program { }