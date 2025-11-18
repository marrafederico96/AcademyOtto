
using AdventureWorks.Data;
using AdventureWorks.Services.CustomerService;
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

            // My services
            builder.Services.AddScoped<ICustomerService, CustomerService>();

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
