using System;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ornament.WebSockets
{
    /// <summary>
    /// </summary>
    public static class OrnamentWebSocketsExtentions
    {
        public static IServiceCollection AddOrnamentWebSocket(this IServiceCollection service,
            Action<OrnamentWebSocketManager> webSocketSetting)
        {
            var manager = new OrnamentWebSocketManager();
            webSocketSetting(manager);
            return service.AddSingleton(manager);
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="webSocketSetting"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOrnamentWebSocket(this IApplicationBuilder app)
        {
            return UseOrnamentWebSocket(app, new WebSocketOptions());
        }

        /// <summary>
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <param name="webSocketSetting"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseOrnamentWebSocket(this IApplicationBuilder app,
            WebSocketOptions options)
        {
            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    try
                    {
                        var webSocketManager = http.RequestServices.GetService<OrnamentWebSocketManager>();
                        if (webSocketManager.GetHandler(http.RequestServices, http.Request.Path, out var handler))
                            await handler.Attach(http, webSocket);
                        else
                            await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation,
                                "没有找到匹配" + http.Request.Path + "的处理器", CancellationToken.None);
                    }
                    catch (Exception ex)
                    {
                        webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message,
                            CancellationToken.None);
                    }
                }
                else
                {
                    await next();
                }
            });


            return app;
        }
    }
}