using Microsoft.OpenApi.Models;
using SizzlingHotProducts.Interfaces;
using SizzlingHotProducts.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IJsonFileAccess>(provider =>
        new JsonFileAccess("Data"));

builder.Services.AddScoped<OrderStatusService, OrderStatusService>();
builder.Services.AddScoped<ISalesConsolidateService, SalesConsolidateService>();
builder.Services.AddScoped<ISalesService, SalesService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bunnings Sizzling Hot Products API",
        Version = "v1",
        Description = "This Api is to get data of top selling products"
    });
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bunnings Sizzling Hot Products API");
    });
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
