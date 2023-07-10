using Microsoft.EntityFrameworkCore;
using PostItDemo.Models;

namespace PostItDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //add database context
            builder.Services.AddDbContext<PostItContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-PostIt;Trusted_Connection=True;MultipleActiveResultSets=true"));

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=PostIts}/{action=Index}/{id?}");

            app.Run();
        }
    }
}