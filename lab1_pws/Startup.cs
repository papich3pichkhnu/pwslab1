using lab1_pws.Services.Helpers.Mails;
using lab1_pws.Services.Helpers.Files;
using lab1_pws.Services.Interfaces.Services;
using lab1_pws.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;

namespace lab1_pws
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            services.AddControllersWithViews();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IFileService, FileService>();
            services.Configure<MailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<FileSettings>(Configuration.GetSection("FileSettings"));
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
           {
                new CultureInfo("en"),
                new CultureInfo("uk")
            };

                options.DefaultRequestCulture = new RequestCulture("uk");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            System.IO.Directory.CreateDirectory("Logs");
            var datetime = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), $"Logs/{datetime}.txt"));
            var logger = loggerFactory.CreateLogger("FileLogger");

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files")),

                RequestPath = new PathString("/Files")
            });

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
            app.UseStaticFiles(new StaticFileOptions() 
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files")),
                RequestPath = new PathString("/Files")
            });

            app.UseRouting();

            app.UseAuthorization();
            app.UseRequestLocalization(app.ApplicationServices.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);


            app.Use(async (context, next) =>
            {
                var request = context.Request;
                var fullUri = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
                var time = DateTime.Now.ToString("HH:mm:ss");
                var ip = context.Connection.RemoteIpAddress?.ToString();
                //logger.LogInformation("Processing request {0}", $"{fullUri} , time: {time}, ip: {ip} ");
                //logger.LogInformation($"CurrentCulture:{CultureInfo.CurrentCulture.Name}, CurrentUICulture:{CultureInfo.CurrentUICulture.Name}") ;
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
