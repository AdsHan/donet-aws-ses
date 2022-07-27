using AmazonSimpleEmail.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration();
builder.Services.AddDependencyConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDistributedCacheConfiguration(builder.Configuration);
builder.Services.AddSettingsConfiguration(builder.Configuration);

var app = builder.Build();

app.UseApiConfiguration();
app.UseSwaggerConfiguration();

app.Run();
