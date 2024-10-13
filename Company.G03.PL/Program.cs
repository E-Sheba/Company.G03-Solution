using Company.G03.BLL;
using Company.G03.BLL.Interfaces;
using Company.G03.DAL.Data.Contexts;
using Company.G03.DAL.Models;
using Company.G03.PL.Mapping.Departments;
using Company.G03.PL.Mapping.Employees;
using Company.G03.PL.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.G03.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();


            //Allow clr to create object from AppDbContext
            //builder.Services.AddScoped<AppDbContext>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
           


            //builder.Services.AddScoped<IDepartmentRepository , DepartmentRepository>(); 
            //builder.Services.AddScoped<IEmployeeRepository , EmployeeRepository>();


            builder.Services.AddScoped<IScopedService,ScopedService>(); //Per Request { if many operations requested a scoped object in one request only one object will be created} 
            builder.Services.AddTransient<ITransientService,TransientService>();//Per operation {a request may contain more than one operation}
            builder.Services.AddSingleton<ISingletonService,SingletonService>();//per App {means until you stop the app from running }


            //Allow Dependancy Injection for IMapper

            builder.Services.AddAutoMapper(typeof(EmployeeProfile));

            builder.Services.AddAutoMapper(typeof(DepartmentProfile));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<AppDbContext>()
                            .AddDefaultTokenProviders();


            builder.Services.ConfigureApplicationCookie(config =>

            config.LoginPath = "/Account/SignIn"
            );

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

            //app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
