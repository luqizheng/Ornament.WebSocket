using System;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ornament.WebSockets;

namespace WebApplication1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrnamentWebSocket();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseOrnamentWebSocket(setting =>
            {
                var handler = setting.AddText("/repeat");
                handler.OnConnecting = (websocket, http, manager) =>
                {
                    websocket.SendTextAsnyc("登录,当前在线人数" + manager.CountClients());
                };
                handler.OnReceived = (websocket, http, text, manager) => { websocket.SendTextAsnyc("系统收到：" + text); };
                handler.OnClosed = (http, manager) =>
                {
                    manager.GetClients().SendTextAsnyc("logout，客户端数目：" + manager.CountClients(), CancellationToken.None);
                };
            });

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseStaticFiles();

        }

    }
}