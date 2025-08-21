using FindSimilar.Application.Abstractions;
using FindSimilar.Application.Services;
using FindSimilar.Infrastructure.Configs;
using FindSimilar.Infrastructure.Providers;
using FindSimilar.Infrastructure.Repositories;
using FindSimilar.Infrastructure.Stores;
using FindSimilar.WebApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using Milvus.Client;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

//Services
builder.Services.AddScoped<IAddressService, AddressService>();

//Repositories
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Milvus Client
builder.Services.AddSingleton<MilvusClient>(_ => new MilvusClient(
		host: builder.Configuration["Milvus:Host"], 
		port: int.Parse(builder.Configuration["Milvus:Port"]),
		ssl: bool.Parse(builder.Configuration["Milvus:UseSSL"])));

//OpenAI Client
builder.Services.AddSingleton<OpenAIClient>(_ => new OpenAIClient(
		apiKey: builder.Configuration["OpenAI:ApiKey"]));

//Providers
builder.Services.AddSingleton<IEmbeddingProvider, OpenAiEmbeddingProvider>();

//Stores 
builder.Services.AddSingleton<IAddressEmbeddingStore, AddressEmbeddingStore>();

//Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "FindSimilar API V1");
		c.RoutePrefix = string.Empty; 
	});
}

app.UseHttpsRedirection();

app.MapAddressEndpoints();

app.Run();
