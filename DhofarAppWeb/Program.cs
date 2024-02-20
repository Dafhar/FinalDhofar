using DhofarAppWeb.Data;
using DhofarAppWeb.Model;
using DhofarAppWeb.Model.Interface;
using DhofarAppWeb.Model.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DhofarAppWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();

            // Add services to the container.
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddControllersWithViews();

            builder.Services.AddSession(options =>
            {
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))

            );

            builder.Services.AddIdentity<User, IdentityRole>
                      (options =>
                        {
                              options.User.RequireUniqueEmail = true;
                       }).AddEntityFrameworkStores<AppDbContext>()
                       .AddDefaultTokenProviders(); ;

           

            builder.Services.AddTransient<IUser, IdentityUserServices>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Create", policy => policy.RequireClaim("Permissions", "Create"));
                options.AddPolicy("Read", policy => policy.RequireClaim("Permissions", "Read"));
                options.AddPolicy("Update", policy => policy.RequireClaim("Permissions", "Update"));
                options.AddPolicy("Delete", policy => policy.RequireClaim("Permissions", "Delete"));
            });

            builder.Services.AddAuthorization();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/User/Login"; // Set the correct path to your login action
            });

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
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            //app.UseCookiePolicy();
            app.MapRazorPages();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}