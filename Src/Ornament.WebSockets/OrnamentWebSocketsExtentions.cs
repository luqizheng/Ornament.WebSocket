using System;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    /// <summary>
    /// </summary>
    public static class OrnamentWebSocketsExtentions
    {
        public static IServiceCollection AddOrnamentWebSocket(this IServiceCollection service)
        {
            return service.AddSingleton<OrnamentWebSocketManager>();
        }
        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="webSocketSetting"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOrnamentWebSocket(this IApplicationBuilder app,
            Action<OrnamentWebSocketManager> webSocketSetting)
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
            WebSocketOptions options, Action<OrnamentWebSocketManager> webSocketSetting)
        {
            if (webSocketSetting == null)
                throw new ArgumentNullException(nameof(webSocketSetting));

            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var manager = http.RequestServices.GetService<OrnamentWebSocketManager>();
                    IWebSocketHandler handler;
                    if (manager.GetHandler(http.Request.Path, out handler))
                    {
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                        await handler.Attach(http, webSocket);
                    }
                    else
                    {
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                        await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation,
                            "没有找到匹配" + http.Request.Path + "的处理器", CancellationToken.None);
                        //throw new OrnamentWebSocketException($"Cannot find WebSocketHandler for " + http.Request.Path);
                    }
                }
                else
                {
                    await next();
                }
            });
            var manager = app.ApplicationServices.GetService<OrnamentWebSocketManager>();
            webSocketSetting(manager);
            return app;
        }
    }
}