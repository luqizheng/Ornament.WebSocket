using System;
using Microsoft.AspNetCore.Http;
using Ornament.WebSockets.Serializer;

namespace Ornament.WebSockets.Handlers
{
    public class TypeHandler : BinaryHandler
    {
        private readonly IWebSocketSerializer _socketSerializer;

        public TypeHandler(int maxBuffer, IWebSocketSerializer socketSerializer) : base(maxBuffer)
        {
            _socketSerializer = socketSerializer;
        }

        public Action<OrnamentWebSocket, WebSocketManager, IWebSocketSerializer> OnReceive { get; set; }

        protected override void ReceiveCompleteData(OrnamentWebSocket socket, HttpContext http, WebSocketManager manager,
            byte[] content)
        {
            OnReceive(socket, manager, _socketSerializer);
        }
    }

}