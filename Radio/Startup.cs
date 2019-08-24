namespace Radio
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Radio.Hubs;
    using Radio.Models;
    using Radio.Models.Database;
    using Radio.Services;

    using WebMarkupMin.AspNetCore2;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _webHostEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
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

            services.AddTransient(_ => _webHostEnvironment.ContentRootFileProvider);
            services.AddTransient<ITrackLoader, TrackLoader>();
            services.AddSingleton<ITrackService, TrackService>();
            services.AddSingleton<ITrackQueue, TrackQueue>();

            services.AddWebMarkupMin(options =>
                    {
                        options.AllowMinificationInDevelopmentEnvironment = true;
                        options.AllowCompressionInDevelopmentEnvironment = true;
                        options.DisablePoweredByHttpHeaders = true;
                    })
                    .AddHtmlMinification(options => options.MinificationSettings.RemoveRedundantAttributes = true)
                    .AddHttpCompression();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseWebMarkupMin();

            app.UseAuthentication();

            app.UseMvc(options =>
            {
                options.MapRoute(name: null,
                                 template: "{Controller=Home}/{Action=Index}");
            });

            app.UseSignalR(routes =>
            {
                routes.MapHub<RadioHub>("/radio");
            });
        }
    }
}