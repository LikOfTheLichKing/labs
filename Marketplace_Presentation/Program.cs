using Marketplace.Application.Interfaces;
using Marketplace.Application.Services;
using Marketplace.Domain.Repositories;
using Marketplace.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI Конфигурация в единой точке
builder.Services.AddSingleton<IAdRepository, InMemoryAdRepository>();
builder.Services.AddSingleton<IUserRepository, InMemoryUserRepository>();
builder.Services.AddScoped<IAdService, AdService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();