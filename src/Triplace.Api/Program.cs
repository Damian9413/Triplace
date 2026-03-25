using Scalar.AspNetCore;
using Triplace.Api.Middleware;
using Triplace.Api.Seeding;
using Triplace.Application.Repositories;
using Triplace.Application.Services;
using Triplace.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        opts.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

builder.Services.AddOpenApi();

// Infrastructure — repositories (singletons so in-memory state persists)
builder.Services.AddSingleton<IAttractionRepository, InMemoryAttractionRepository>();
builder.Services.AddSingleton<IAttractionGroupRepository, InMemoryAttractionGroupRepository>();
builder.Services.AddSingleton<IAttractionAddonTypeRepository, InMemoryAttractionAddonTypeRepository>();
builder.Services.AddSingleton<ISeasonalCatalogRepository, InMemorySeasonalCatalogRepository>();
builder.Services.AddSingleton<IRouteRepository, InMemoryRouteRepository>();
builder.Services.AddSingleton<IAttractionRelationRegistryRepository, InMemoryAttractionRelationRegistryRepository>();

// Application services
builder.Services.AddScoped<AttractionService>();
builder.Services.AddScoped<AttractionGroupService>();
builder.Services.AddScoped<AttractionAddonTypeService>();
builder.Services.AddScoped<SeasonalCatalogService>();
builder.Services.AddScoped<RouteService>();
builder.Services.AddScoped<RelationService>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    await DataSeeder.SeedAsync(scope.ServiceProvider);
}

app.Run();
