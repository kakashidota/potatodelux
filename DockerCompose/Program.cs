
using DockerCompose.Data;
using DockerCompose.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerCompose
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProductContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using (var scope = app.Services.CreateScope()) 
            { 
                var context = scope.ServiceProvider.GetRequiredService<ProductContext>();
                context.Database.Migrate();

                if (!context.Products.Any())
                {
                    context.Products.AddRange(
                        new Product { Name = "Fanta", Price = 10},
                        new Product { Name = "Cola", Price = 15},
                        new Product { Name = "Bebsi", Price = 19}
                        );

                    context.SaveChanges();
                }
            }

            app.MapGet("/products", async (ProductContext db) => await db.Products.ToListAsync());

            app.Run();
        }
    }
}
