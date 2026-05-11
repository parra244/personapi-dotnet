using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using personapi_dotnet.DAL.Interfaces;
using personapi_dotnet.DAL.Repositories;
using personapi_dotnet.Models;

var builder = WebApplication.CreateBuilder(args);

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<PersonaDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.EnableRetryOnFailure()
    ));

// ── DAO / Repository pattern (Dependency Injection) ──────────────────────────
builder.Services.AddScoped<IPersonaRepository,   PersonaRepository>();
builder.Services.AddScoped<IProfesionRepository, ProfesionRepository>();
builder.Services.AddScoped<IEstudioRepository,   EstudioRepository>();
builder.Services.AddScoped<ITelefonoRepository,  TelefonoRepository>();

// ── MVC + API controllers ─────────────────────────────────────────────────────
builder.Services.AddControllersWithViews()
    .AddJsonOptions(opt =>
        opt.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "PersonaAPI",
        Version     = "v1",
        Description = "API REST para gestión de personas, profesiones, estudios y teléfonos",
        Contact     = new OpenApiContact { Name = "Lab Arquitectura de Software" }
    });
    // Include XML comments if generated
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// ── Middleware pipeline ───────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PersonaAPI v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// MVC routes
app.MapGet("/", () => Results.Redirect("/swagger"));

// API routes (attribute routing)
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
