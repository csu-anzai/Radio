namespace Radio
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Radio.Hubs.Radio;
    using Radio.Models.Database;
    using Radio.Models.Repositories;
    using Radio.Models.User;
    using Radio.Services;
    using Radio.Services.FileProviders;

    using Swashbuckle.AspNetCore.Swagger;

    using WebMarkupMin.AspNetCore2;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_configuration["Data:ConnectionString"]));

            services.AddIdentity<AppUser, IdentityRole>(options =>
                    {
                        options.User.AllowedUserNameCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-._@/ ";
                        options.Password.RequireLowercase = false;
                    })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddMvc();
            services.AddSignalR();
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp/dist");

            services.AddTransient<IWebRootFileProvider, WebRootFileProvider>(serviceProvider => new WebRootFileProvider(_hostingEnvironment.WebRootFileProvider));
            services.AddTransient<IUserImageService, UserImageService>();

            services.AddTransient<IChannelRepository, ChannelRepository>();
            services.AddTransient<IChannelTrackRepository, ChannelTrackRepository>();

            services.AddTransient<IRadioHubProxy, RadioHubProxy>();

            services.AddSingleton<TrackStatusService>();
            services.AddSingleton<ITrackStatusService>(serviceProvider => serviceProvider.GetService<TrackStatusService>());
            services.AddHostedService<TrackService>();

            services.AddWebMarkupMin(options =>
                    {
                        options.AllowMinificationInDevelopmentEnvironment = true;
                        options.AllowCompressionInDevelopmentEnvironment = true;
                        options.DisablePoweredByHttpHeaders = true;
                    })
                    .AddHtmlMinification(options => options.MinificationSettings.RemoveRedundantAttributes = true)
                    .AddHttpCompression();

            services.AddSwaggerGen(options => options.SwaggerDoc("v1",
                                                                 new Info
                                                                 {
                                                                     Title = "Radio API",
                                                                     Version = "v1"
                                                                 }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Radio API v1"));
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseWebMarkupMin();

            app.UseAuthentication();

            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<RadioHub>("/radio");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080/");
                }
            });
        }
    }
}