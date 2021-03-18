namespace Airport.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting; 
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;

    using Common.Automapper;
    using Common.Constants;

    using Data;
    using Data.Models;

    using Web.Infrastructure.Extensions;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AirportDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString(WebConstants.DbConnection.DefaultConnection)));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = DataConstants.User.PasswordMinLength;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;

                options.User.RequireUniqueEmail = true;
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AirportDbContext>()
                .AddDefaultTokenProviders();

            services.AddDomainServices();

            services.AddAutoMapper(opt => opt.AddProfile(new AutoMapperProfile()));

            services.AddMvc(options =>
                            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDatabaseMigration();
            app.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(WebConstants.Routing.HomeError);
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{Controller=Home}/{action=Index}/{id?}"); 
            });
        }
    }
}
