// src/DeveloperStore.Sales.API/Program.cs

using DeveloperStore.Sales.Application.Commands.CreateBranch;
using DeveloperStore.Sales.Application.Commands.CreateCustomer;
using DeveloperStore.Sales.Application.Commands.CreateProduct;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Queries.CalculateDiscount; // <-- certifique-se de que este namespace está correto
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Services;
using DeveloperStore.Sales.Infrastructure.Context;
using DeveloperStore.Sales.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 1) Controllers
builder.Services.AddControllers();

// 2) Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales API",
        Version = "v1",
        Description = "API para gerenciamento de vendas, produtos, filiais e clientes"
    });
});

// 3) EF Core (SQLite)
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseSqlite("Data Source=sales.db"));

// 4) Serviços de domínio
builder.Services.AddScoped<IDiscountCalculator, DiscountCalculator>();

// 5) Repositórios
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 6) Handlers de comando
builder.Services.AddScoped<CreateSaleCommandHandler>();
builder.Services.AddScoped<CreateBranchCommandHandler>();
builder.Services.AddScoped<CreateCustomerCommandHandler>();
builder.Services.AddScoped<CreateProductCommandHandler>();

// 7) Handler de consulta de desconto (usado pelo DiscountsController)
builder.Services.AddScoped<CalculateDiscountQueryHandler>();

// 8) CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// 9) Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
