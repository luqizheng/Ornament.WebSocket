using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    /// <summary>
    /// 
    /// </summary>
    public static class OrnamentWebSocketsExtentions
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="webSocketSetting"></param>
        /// <returns></returns>
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
            app.UseWebSockets();

            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                  
                    WebSocketHandler handler;
                    if (OrnamentWebSocketManager.Instance.GetHandler(http.Request.Path,out handler))
                    {
                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
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
           
            webSocketSetting(new WebSocketHandlerBuilder());
            return app;
        }
    }
}