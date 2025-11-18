
using AdventureWorks.Data;
using AdventureWorks.Services.CustomerService;
using AdventureWorks.Services.ProductService;
using AuthLibrary;
using AuthLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

namespace AdventureWorks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AdventureWorksLt2019Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AdventureWorks")
                    ?? throw new InvalidOperationException("Connection string not found"));
            });

            // Add Auth Library 
            var connectionStringSecurity = builder.Configuration.GetConnectionString("AdventureWorksSecurity")
                ?? throw new InvalidOperationException("Connection string not found");

            var tokenSettings = builder.Configuration.GetSection("TokenSettings").Get<TokenSettings>()
                ?? throw new InvalidOperationException("Token settings not found");

            SqlService sqlService = new(connectionStringSecurity, tokenSettings);
            builder.Services.AddSingleton(sqlService);

            // My services
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<IProductService, ProductService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapSwagger("/openapi/{documentName}.json");
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
