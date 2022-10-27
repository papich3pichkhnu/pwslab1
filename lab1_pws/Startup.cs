using lab1_pws.Services.Helpers.Mails;
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
            services.AddControllersWithViews();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<MailSettings>(Configuration.GetSection("EmailSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var datetime = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss");
            loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), $"Logs/{datetime}.txt"));
            var logger = loggerFactory.CreateLogger("FileLogger");

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

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                var request=context.Request;
                var fullUri = $"{request.Scheme}://{request.Host}{request.PathBase}{request.Path}{request.QueryString}";
                var time = DateTime.Now.ToString("HH:mm:ss");
                var ip = context.Connection.RemoteIpAddress?.ToString();
                logger.LogInformation("Processing request {0}", $"{fullUri} , time: {time}, ip: {ip}");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });           
        }       
    }
}
