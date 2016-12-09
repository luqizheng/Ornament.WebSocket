using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ornament.WebSockets.WebSocketProtocols;

namespace Ornament.WebSockets.WebSocketHandlers
{
    public sealed class WebSocketHandler

    {
        private readonly IWebSocketProtocol _protocol;

        public WebSocketHandler(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            ServiceProvider = serviceProvider;
        }

        public WebSocketHandler(IServiceProvider serviceProvider, IWebSocketProtocol protocol)
            : this(serviceProvider)
        {
            if (protocol == null) throw new ArgumentNullException(nameof(protocol));
            _protocol = protocol;
        }

        private Action<OrnamentWebSocket, HttpContext, byte[]> ReceivedAction { get; set; }
        internal WebSocketManager WebSocketManager { get; set; }
        public IServiceProvider ServiceProvider { get; }
        public event EventHandler<WebSocketArgs> Closed;

        public event EventHandler<WebSocketArgs> Connecting;

        public event EventHandler<WebSocketReceivedArgs> Received;

        internal async Task Attach(HttpContext http, WebSocket socket)
        {
            var oWebSocket = WebSocketManager.AddNewSocket(socket, _protocol);

            OnConnecting(oWebSocket, http);

            var buffer = new ArraySegment<byte>(new byte[4096]);

            while (socket.State == WebSocketState.Open)
            {
                var socketReceiveResult = await socket.ReceiveAsync(buffer, CancellationToken.None);
                if (socketReceiveResult.MessageType == WebSocketMessageType.Close)
                    break;

                ReceivedAction?.Invoke(oWebSocket, http, buffer.ToArray());
            }

            OnClosed(oWebSocket, http);
        }

        private void OnClosed(OrnamentWebSocket oWebSocket, HttpContext http)
        {
            WebSocketManager.Remove(oWebSocket.Id);
            Closed?.Invoke(this, new WebSocketArgs(oWebSocket, http));
        }

        private void OnConnecting(OrnamentWebSocket oWebSocket, HttpContext http)
        {
            Connecting?.Invoke(this, new WebSocketArgs(oWebSocket, http));
        }

        private void OnReceived(OrnamentWebSocket oWebSocket, HttpContext http, byte[] content)
        {
            Received?.Invoke(this, new WebSocketReceivedArgs(oWebSocket, http, content));
        }
    }
}