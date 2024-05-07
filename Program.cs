using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Context;
using OnlineShoppingApp.DataSeed.ShoppingSeed;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Repositories.Classes;
using OnlineShoppingApp.Repositories.Interfaces;
using OnlineShoppingApp.Services;
using OnlineShoppingApp.Services.Classes;
using OnlineShoppingApp.Services.Interfaces;
using Stripe;
using static System.Formats.Asn1.AsnWriter;

namespace OnlineShoppingApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // 1.Adding dbcontext to the container
            builder.Services.AddDbContext<ShoppingContext>(op =>
                  op.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"))
            );
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICategoriesRepo, CategoryRepo>();
            builder.Services.AddScoped<IBrandRepo, BrandRepo>();
            builder.Services.AddScoped<ICommentsRepo, CommentsRepo>();
            builder.Services.AddScoped<IRateRepo, RateRepo>();
            builder.Services.AddScoped<IBuyerRepo, BuyerRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();


            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            builder.Services.AddScoped<IDeliveryMethodsRepo, DeliveryMethodRepo>();
            builder.Services.AddScoped<IAddressRepo, AddressRepo>();
            builder.Services.AddScoped<ISellerRepo, SellerRepo>();
            builder.Services.AddScoped<IReplyRepo, ReplyRepo>();


            // 2.Adding identity to the container
            builder.Services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<ShoppingContext>()
                .AddDefaultTokenProviders();


            // Add services to the container.
            builder.Services.AddControllersWithViews();
           builder.Services.AddHttpContextAccessor();
           builder.Services.AddScoped<CartService>();
           


            // 3. adding google authentication to the container
            builder.Services.AddAuthentication()
                  .AddGoogle(options =>
                  {
                      options.ClientId = builder.Configuration["GoogleKeys:ClientId"];
                      options.ClientSecret = builder.Configuration["GoogleKeys:ClientSecret"]; ;
                  });


            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();
               



            // 5. add authentication
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
               name: "CatchAll",
               pattern: "{*url}",
               defaults: new { controller = "Home", action = "NotFound" }
           );
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var _loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var _dbContext = services.GetRequiredService<ShoppingContext>();
            try
            {
                 await _dbContext.Database.MigrateAsync(); // Update-Database
                 await ShoppingContextSeed.SeedAsync(_dbContext); // Data Seeding
            }
            catch (Exception ex)
            {
                var logger = _loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error has Occured during apply the migration");
            }
            app.Run();
        }
    }
}
