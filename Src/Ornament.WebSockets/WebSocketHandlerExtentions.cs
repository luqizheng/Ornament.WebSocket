using System;
using Ornament.WebSockets.WebSocketHandlers;
using Ornament.WebSockets.WebSocketProtocols;

namespace Ornament.WebSockets
{
    public static class WebSocketHandlerExtentions
    {
        public static WebSocketHandler ReceivedByProtocol<TWebSocketSerializer, TObject>(
            this WebSocketHandler handler,
            Action<WebSocketArgs, TObject> func)
        {
            handler.Received += (sender, args) =>
            {
                var serializer =
                    handler.ServiceProvider.GetService(typeof(TWebSocketSerializer)) as IWebSocketProtocol;
                if (serializer == null)
                    throw new Exception("serializer is not found, please use add proptoy");
                var tobject = serializer.GetObject<TObject>(args.Content);

                func(args, tobject);
            };
            return handler;
        }
    }
}