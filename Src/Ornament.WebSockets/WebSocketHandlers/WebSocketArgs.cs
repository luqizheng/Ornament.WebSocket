using System;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.WebSocketHandlers
{
    public class WebSocketArgs : EventArgs
    {
        public WebSocketArgs(OrnamentWebSocket webSocket, HttpContext http)
        {
            WebSocket = webSocket;
            Http = http;
        }

        public OrnamentWebSocket WebSocket { get; set; }
        public HttpContext Http { get; set; }
    }
}