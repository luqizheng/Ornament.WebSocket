using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ornament.WebSockets;
using WebApplication1.Models;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
         
            app.UseWebSockets();

            app.UseOrnamentWebSocket(setting =>
            {
                //text send and reply,client code on quickStart.html;
                setting.Demo1();
                //upload file demo; client code on uploadFile.html
                setting.FileUploadDemo();
            });

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();


            app.UseDefaultFiles();
            app.UseStaticFiles();
        }
    }
}