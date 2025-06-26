using Microsoft.EntityFrameworkCore;
using PersonApi.Data;
using PersonApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add controllers to the container.
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonTypeService, PersonTypeService>();
builder.Services.AddEndpointsApiExplorer();
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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.UseCors(allowSpeficOrigins);
app.Run();

public partial class Program { }