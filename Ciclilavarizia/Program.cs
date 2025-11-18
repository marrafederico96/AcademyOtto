
using AdventureWorks.Data;
using AdventureWorks.Exceptions;
using AdventureWorks.Services.CustomerService;
using AdventureWorks.Services.ProductService;
using AuthLibrary;
using AuthLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

namespace AdventureWorks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionStringProd = builder.Configuration.GetConnectionString("AdventureWorks")
                    ?? throw new InvalidOperationException("Connection string not found");
            var connectionStringSecurity = builder.Configuration.GetConnectionString("AdventureWorksSecurity")
                    ?? throw new InvalidOperationException("Connection string not found");

            //Add Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("Logs\\ciclilavarizia.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Add services to the container.
            builder.Services.AddDbContext<AdventureWorksLt2019Context>(options =>
            {
                options.UseSqlServer(connectionStringProd);
            });

            // Add Middleware Exception
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // Add Auth Library 
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

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
