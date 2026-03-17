using Triplace.Application.Attractions;
using Triplace.Domain.Repositories;
using Triplace.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Domain
builder.Services.AddScoped<IAttractionRepository, InMemoryAttractionRepository>();

// Application
builder.Services.AddScoped<GetAttractionsByCity>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
