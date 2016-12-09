using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.WebSocketHandlers
{
    public class WebSocketReceivedArgs : WebSocketArgs
    {
        public WebSocketReceivedArgs(OrnamentWebSocket webSocket, HttpContext http, byte[] content)
            : base(webSocket, http)
        {
            Content = content;
        }

        public byte[] Content { get; }
    }
}