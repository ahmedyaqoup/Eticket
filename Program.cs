using Eticket.Data;
using Eticket.Models;
using Eticket.Repository;
using Eticket.Repository.IRepository;
using Eticket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Eticket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<ApplicationDbcontext>(
               option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
               );

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbcontext>()
           .AddDefaultTokenProviders();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICinemaRepository ,CinemaRepository>();
            builder.Services.AddScoped<IActorRepository, ActorRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
