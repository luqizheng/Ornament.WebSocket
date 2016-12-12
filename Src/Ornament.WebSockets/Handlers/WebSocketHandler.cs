using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public abstract class WebSocketHandler
    {
        public Action<HttpContext, WebSocketManager> OnClosed;
        public Action<OrnamentWebSocket, HttpContext, WebSocketManager> OnConnecting;


        protected WebSocketHandler(int buffSize = 4096)
        {
            BuffSize = buffSize;
        }


        internal WebSocketManager WebSocketManager { get; set; }


        public int BuffSize { get; }

        internal async Task Attach(HttpContext http, WebSocket socket)
        {
            var oWebSocket = WebSocketManager.AddNewSocket(socket);

            OnConnecting?.Invoke(oWebSocket, http, WebSocketManager);

            while (socket.State == WebSocketState.Open)
            {
                var buffer = new ArraySegment<byte>(new byte[BuffSize]);
                var socketReceiveResult = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socketReceiveResult.MessageType == WebSocketMessageType.Close)
                    break;

                OnReceivedData(oWebSocket, http, buffer.ToArray(), socketReceiveResult, WebSocketManager);
            }
            WebSocketManager.Remove(oWebSocket.Id);
            OnClosed?.Invoke(http, WebSocketManager);
        }


        protected abstract void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult, WebSocketManager manager);
    }
}