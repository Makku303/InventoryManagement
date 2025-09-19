using Core.IRepositories;
using Core.IServices;
using Core.Models;
using Core.Models.Enums;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServiceLayer;
using System.Text;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IInventoryService, InventoryService>();
            builder.Services.AddScoped<IDashboardService, DashboardService>();
            builder.Services.AddScoped<IReportingService, ReportingService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();

            //database context
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Inventory API",
                    Version = "v1",
                    Description = "API to manage inventory"
                });

                // Add JWT Bearer auth to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid JWT token.\n\nExample: Bearer 12345abcdef"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // JWT Authentication
            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });


            // Add CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularDev", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await SeedDatabaseAsync(dbContext);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryManagement API v1");
                    c.RoutePrefix = string.Empty; // Makes Swagger UI the default page
                });
            }
            app.UseCors("AllowAngularDev");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static async Task SeedDatabaseAsync(ApplicationDbContext context)
        {
            var userId = new Guid("57CF5646-2604-446D-8CF6-08DDEC42FA2D"); // Replace with actual user ID

            if (!context.Categories.Any())
            {
                var categories = Enumerable.Range(1, 10).Select(i => new Category
                {
                    Name = $"Category {i}",
                    Description = $"Description for Category {i}",
                    IsActive = true
                }).ToList();
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            if (!context.Suppliers.Any())
            {
                var suppliers = Enumerable.Range(1, 10).Select(i => new Supplier
                {
                    Name = $"Supplier {i}",
                    ContactEmail = $"supplier{i}@example.com",
                    ContactPhone = $"0917{i:D7}",
                    Address = $"Address {i}",
                    Notes = $"Notes for Supplier {i}",
                    IsActive = true
                }).ToList();
                context.Suppliers.AddRange(suppliers);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var categories = context.Categories.ToList();
                if (categories.Count == 0) throw new Exception("No categories found for product seeding.");

                var products = Enumerable.Range(1, 10).Select(i => new Product
                {
                    Name = $"Product {i}",
                    SKU = $"SKU{i:D4}",
                    Description = $"Description for Product {i}",
                    UnitPrice = 100 + i,
                    ReorderLevel = 5,
                    QuantityOnHand = 50,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    CategoryId = categories[i % categories.Count].Id
                }).ToList();
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            if (!context.Purchases.Any())
            {
                var suppliers = context.Suppliers.ToList();
                var purchases = Enumerable.Range(1, 10).Select(i => new Purchase
                {
                    InvoiceNumber = $"INV-PUR-{i:D4}",
                    PurchaseDate = DateTime.UtcNow.AddDays(-i),
                    TotalAmount = 1000 + i * 10,
                    CreatedById = userId,
                    SupplierId = suppliers[i % suppliers.Count].Id,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
                context.Purchases.AddRange(purchases);
                await context.SaveChangesAsync();
            }

            if (!context.PurchaseItems.Any())
            {
                var purchases = context.Purchases.ToList();
                var products = context.Products.ToList();
                var purchaseItems = purchases.SelectMany(p => products.Select(prod => new PurchaseItem
                {
                    PurchaseId = p.Id,
                    ProductId = prod.Id,
                    Quantity = 10,
                    LineTotal = prod.UnitPrice * 10
                })).ToList();
                context.PurchaseItems.AddRange(purchaseItems);
                await context.SaveChangesAsync();
            }

            if (!context.Sales.Any())
            {
                var sales = Enumerable.Range(1, 10).Select(i => new Sale
                {
                    CustomerName = $"Customer {i}",
                    InvoiceNumber = $"INV-SALE-{i:D4}",
                    SaleDate = DateTime.UtcNow.AddDays(-i),
                    TotalAmount = 1200 + i * 15,
                    CreatedById = userId,
                    CreatedAt = DateTime.UtcNow
                }).ToList();
                context.Sales.AddRange(sales);
                await context.SaveChangesAsync();
            }

            if (!context.SaleItems.Any())
            {
                var sales = context.Sales.ToList();
                var products = context.Products.ToList();
                var saleItems = sales.SelectMany(s => products.Select(prod => new SaleItem
                {
                    SaleId = s.Id,
                    ProductId = prod.Id,
                    Quantity = 5,
                    LineTotal = prod.UnitPrice * 5
                })).ToList();
                context.SaleItems.AddRange(saleItems);
                await context.SaveChangesAsync();
            }

            if (!context.InventoryTransactions.Any())
            {
                var products = context.Products.ToList();
                var purchases = context.Purchases.ToList();
                var inventoryTransactions = products.Select(prod => new InventoryTransaction
                {
                    ProductId = prod.Id,
                    ChangeQuantity = 10,
                    PreviousQuantity = prod.QuantityOnHand,
                    NewQuantity = prod.QuantityOnHand + 10,
                    TransactionType = TransactionType.Purchase,
                    ReferenceId = purchases.First().Id,
                    PerformedAt = DateTime.UtcNow,
                    Notes = "Initial stock",
                    PerformedById = userId
                }).ToList();
                context.InventoryTransactions.AddRange(inventoryTransactions);
                await context.SaveChangesAsync();
            }
        }
    }
}
