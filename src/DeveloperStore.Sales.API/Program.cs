

using DeveloperStore.Sales.Application.Commands.CreateBranch;
using DeveloperStore.Sales.Application.Commands.CreateCustomer;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Services;
using DeveloperStore.Sales.Infrastructure.Context;
using DeveloperStore.Sales.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        Description = "API para gerenciamento de vendas, filiais e clientes"
    });
});

// 3) EF Core + SQLite
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseSqlite("Data Source=sales.db"));

// 4) Serviços de Domínio
builder.Services.AddScoped<IDiscountCalculator, DiscountCalculator>();

// 5) Repositórios
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

// 6) Handlers de Comando
builder.Services.AddScoped<CreateSaleCommandHandler>();
builder.Services.AddScoped<CreateBranchCommandHandler>();
builder.Services.AddScoped<CreateCustomerCommandHandler>();

var app = builder.Build();

// 7) Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
