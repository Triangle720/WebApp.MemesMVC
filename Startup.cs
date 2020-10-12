using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.MemesMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApp.MemesMVC
{
    public class Startup
    {
        private readonly string _secret;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _secret = configuration.GetSection("JWT").GetSection("secret").Value;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<DatabaseContext>(options =>
            {
                //options.UseLazyLoadingProxies().UseSqlServer(Configuration["ConnectionString:Memes"]); local env
                options.UseLazyLoadingProxies().UseSqlServer(Configuration["AzureConnectionString:Memes"]); //azure test
            });

            services.AddSession();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "INO",
                        IssuerSigningKey = symmetricKey
                    };
                });

            services.AddAuthorization();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
