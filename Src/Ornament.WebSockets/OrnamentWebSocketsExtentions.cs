using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ornament.WebSockets.WebSocketHandlers;
using Ornament.WebSockets.WebSocketProtocols;

namespace Ornament.WebSockets
{
    public static class OrnamentWebSocketsExtentions
    {
        /// <summary>
        ///     Add OrnamentWebSocket
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddOrnamentWebSocket(this IServiceCollection services)

        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services
                .AddSingleton<WebSocketManager>()
                .AddSingleton<WebSocketHandlerManager>();

            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebSockeProtocol<T>(this IServiceCollection services)
            where T : IWebSocketProtocol

        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddTransient(typeof(T));
            return services;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="webSocketSetting"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOrnamentWebSocket(this IApplicationBuilder app,
            Action<WebSocketHandlerBuilder> webSocketSetting)
        {
            if (webSocketSetting == null)
                throw new ArgumentNullException(nameof(webSocketSetting));
            app
                .UseWebSockets()
            .Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var manager = app.ApplicationServices.GetService<WebSocketHandlerManager>();
                    if (manager.ContainsHandler(http.Request.Path))
                    {
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                        var handler = manager.GetHandler(http.Request.Path);
                        await handler.Attach(http, webSocket);
                    }
                }
                else
                {
                    await next();
                }
            });
            var handlerManager = app.ApplicationServices.GetService<WebSocketHandlerManager>();
            webSocketSetting(new WebSocketHandlerBuilder(app.ApplicationServices, handlerManager));
            return app;
        }
    }
}