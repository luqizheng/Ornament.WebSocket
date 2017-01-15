using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ornament.WebSockets
{
    public static class OrnamentWebSocketsExtentions
    {
        /// <summary>
        ///     AddIn OrnamentWebSocket
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOrnamentWebSocket(this IServiceCollection services)

        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSingleton<WebSocketManager>();
            return services;
        }

        public static IApplicationBuilder UseOrnamentWebSocket(this IApplicationBuilder app,
            Action<WebSocketHandlerBuilder> webSocketSetting)
        {
            return UseOrnamentWebSocket(app, new WebSocketOptions(), webSocketSetting);
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <param name="webSocketSetting"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOrnamentWebSocket(this IApplicationBuilder app,
            WebSocketOptions options, Action<WebSocketHandlerBuilder> webSocketSetting)
        {
            if (webSocketSetting == null)
                throw new ArgumentNullException(nameof(webSocketSetting));


            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var manager = app.ApplicationServices.GetService<WebSocketManager>();
                    if (manager.ContainsHandler(http.Request.Path))
                    {
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                        var handler = manager.GetHandler(http.Request.Path);
                        await handler.Attach(http, webSocket);
                    }
                    else
                    {
                        throw new OrnamentWebSocketException($"Cannot find WebSocketHandler for " + http.Request.Path);
                    }
                }
                else
                {
                    await next();
                }
            });
            var socketManager = app.ApplicationServices.GetService<WebSocketManager>();
            webSocketSetting(new WebSocketHandlerBuilder(socketManager));
            return app;
        }
    }
}