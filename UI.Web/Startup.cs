using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NsdcTraingPartnerHub.Core.Interfaces;
using NsdcTraingPartnerHub.Data;
using NsdcTraingPartnerHub.Data.Repository;
using NsdcTraingPartnerHub.Service.Configuration;
using NsdcTraingPartnerHub.Service.Configurations;
using NsdcTraingPartnerHub.Utility;
using Serilog.Context;
using Stripe;

namespace NsdcTraingPartnerHub.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NsdcTpDbContext>(options =>
            {
                options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(
                        Configuration.GetConnectionString("DefaultConnection"));
            });

            services.Configure<StripeSetting>(Configuration.GetSection("Stripe")); // Configiring Stripe Settings

            services.AddIdentity<IdentityUser, IdentityRole>(
                options => options.SignIn.RequireConfirmedAccount = true
                ).AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

            services.Configure<MailClientConfigurationOption>(Configuration.GetSection("MailClientConfigurationOption"));

            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddAutoMapper(typeof(MapperInitializer));

            services.AddControllersWithViews();

            services.AddRazorPages().AddRazorRuntimeCompilation();
            
            services.ConfigureApplicationCookie(options =>
             {
                 options.LoginPath = $"/Identity/Account/Login";
                 options.AccessDeniedPath = $"/Identity/Account/Logout";
                 options.LogoutPath = $"/Identity/Account/AccessDenied";
             });

            // Caching Configuration
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddResponseCaching();

            services.AddSession(options => {
                options.IdleTimeout = System.TimeSpan.FromMinutes(100);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ConfigureExceptionHandler(); // Configuring Global Error handling and Logging

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe:SecretKey").Get<string>();

            app.UseAuthentication(); // Configuring user Authentication

            app.Use(async (httpContext, next) =>
            {
                var userName = httpContext.User.Identity.IsAuthenticated ? httpContext.User.Identity.Name : "Guest"; //Gets user Name from user Identity  
                LogContext.PushProperty("Username", userName); //Push user in LogContext;  
                await next.Invoke();
            });

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area=TrainingCenter}/{controller=Home}/{action=Index}/{id?}"
                    );
            });
        }
    }
}
