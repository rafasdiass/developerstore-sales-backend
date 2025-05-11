using DeveloperStore.Sales.Application.Commands.CreateBranch;
using DeveloperStore.Sales.Application.Commands.CreateCustomer;
using DeveloperStore.Sales.Application.Commands.CreateProduct;
using DeveloperStore.Sales.Application.Commands.CreateSale;
using DeveloperStore.Sales.Application.Commands.UpdateSale;
using DeveloperStore.Sales.Application.Commands.DeleteSale;
using DeveloperStore.Sales.Application.Queries.CalculateDiscount;
using DeveloperStore.Sales.Application.Repositories;
using DeveloperStore.Sales.Domain.Services;
using DeveloperStore.Sales.Infrastructure.Context;
using DeveloperStore.Sales.Infrastructure.Repositories;
using DeveloperStore.Sales.API.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────────────────────
// 1) Logging
// ─────────────────────────────────────────────────────────────────────────────
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// ─────────────────────────────────────────────────────────────────────────────
// 2) Services
// ─────────────────────────────────────────────────────────────────────────────

// 2.1) Controllers
builder.Services.AddControllers();

// 2.2) Swagger com suporte a PATCH
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales API",
        Version = "v1",
        Description = "API para gerenciamento de vendas, produtos, filiais e clientes"
    });

    c.SupportNonNullableReferenceTypes();
    c.OperationFilter<PatchOperationFilter>();
});

// 2.3) EF Core (SQLite)
builder.Services.AddDbContext<SalesDbContext>(options =>
    options.UseSqlite("Data Source=sales.db")
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging()
);

// 2.4) Serviços de domínio
builder.Services.AddScoped<IDiscountCalculator, DiscountCalculator>();

// 2.5) Repositórios
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// 2.6) Handlers de comandos
builder.Services.AddScoped<CreateSaleCommandHandler>();
builder.Services.AddScoped<UpdateSaleCommandHandler>();
builder.Services.AddScoped<DeleteSaleCommandHandler>();
builder.Services.AddScoped<CreateBranchCommandHandler>();
builder.Services.AddScoped<CreateCustomerCommandHandler>();
builder.Services.AddScoped<CreateProductCommandHandler>();

// 2.7) Handlers de consultas
builder.Services.AddScoped<CalculateDiscountQueryHandler>();

// 2.8) CORS — política nomeada e robusta
builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCors", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ─────────────────────────────────────────────────────────────────────────────
// 3) App / Pipeline HTTP
// ─────────────────────────────────────────────────────────────────────────────

var app = builder.Build();

// 3.1) Ambiente de desenvolvimento
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales API v1");
        c.RoutePrefix = string.Empty;
    });
}

// 3.2) Middlewares
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("DefaultCors");
app.UseAuthorization();

// 3.3) Endpoints
app.MapControllers();

// 3.4) Start
app.Run();
