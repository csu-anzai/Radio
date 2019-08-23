namespace Radio
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Radio.Hubs;
    using Radio.Models;
    using Radio.Models.Database;
    using Radio.Services;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseNpgsql(_configuration["Data:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>(options =>
                    {
                        options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-._@/ ";
                        options.Password.RequireLowercase = false;
                    })
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddSignalR();

            services.AddSingleton<ITrackService, TrackService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: null,
                                             pattern: "{Controller=Home}/{Action=Index}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<RadioHub>("/radio");
            });
        }
    }
}