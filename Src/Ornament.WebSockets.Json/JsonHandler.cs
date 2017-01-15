using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets.Json
{
    public class JsonHandler : WebSocketHandler
    {
        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult,
            WebSocketHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}