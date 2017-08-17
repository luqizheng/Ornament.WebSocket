using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ornament.WebSockets.Collections;

namespace Ornament.WebSockets.Handlers
{
    public interface IWebSocketHandler
    {
        IEnumerable<OrnamentWebSocket> GetClients();
        int CountClients();

        bool TryGetWebSocket(string id, out OrnamentWebSocket weboSocket);
        Task Attach(HttpContext http, WebSocket socket);
    }

    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WebSocketHandler : IWebSocketHandler

    {
        public static string DefaultGroupName = "DEFAULT";
        private readonly WebSocketCollection _webSockets = new WebSocketCollection();

        private WebSocketGroupColllection _groups;

        //public Action<OrnamentWebSocket, HttpContext, WebSocketHandler> OnClosed;
        //public Action<OrnamentWebSocket, HttpContext, WebSocketHandler> OnConnecting;
        protected WebSocketOptions Options;

        protected WebSocketHandler(IOptions<WebSocketOptions> options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Options = options.Value;
        }

        //public OrnamentWebSocketManager OrnamentWebSocketManager { get; internal set; }

        public WebSocketGroupColllection Groups => _groups ?? (_groups = new WebSocketGroupColllection());

        public int CountClients()
        {
            return _webSockets.CountClients();
        }

        public IEnumerable<OrnamentWebSocket> GetClients()
        {
            return _webSockets.GetClients();
        }

        private OrnamentWebSocket Add(WebSocket socket)
        {
            var result = new OrnamentWebSocket(socket, this);
            _webSockets.Add(result);
            return result;
        }

        public async Task Attach(HttpContext http, WebSocket socket)
        {
            var oWebSocket = Add(socket);

            OnConnecting(oWebSocket, http);

            var group = GroupupSocket(oWebSocket, http);
            if (string.IsNullOrEmpty(group))
                group = DefaultGroupName;

            this.Groups.Add(oWebSocket, group);

            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[Options.ReceiveBufferSize]);
                    var socketReceiveResult = await socket.ReceiveAsync(buffer, CancellationToken.None);
                    if (socketReceiveResult.MessageType == WebSocketMessageType.Close)
                        break;

                    OnReceivedData(oWebSocket, http, buffer.ToArray(), socketReceiveResult, this);
                }
            }
            catch (Exception ex)
            {
                await socket.CloseAsync(WebSocketCloseStatus.InternalServerError, ex.Message, CancellationToken.None);
                throw;
            }
            finally
            {
                _webSockets.Remove(oWebSocket.Id);
                Groups.Remove(oWebSocket);
                OnClosing(oWebSocket, http);
            }
        }


        protected abstract void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult, WebSocketHandler handler);

        public bool TryGetWebSocket(string id, out OrnamentWebSocket weboSocket)
        {
            weboSocket = _webSockets.Get(id);
            return weboSocket != null;
        }
        /// <summary>
        /// 为ornament 进行分组。
        /// </summary>
        /// <param name="ornamentWeb"></param>
        /// <param name="http"></param>
        /// <returns></returns>
        protected virtual string GroupupSocket(OrnamentWebSocket ornamentWeb, HttpContext http)
        {
            return "";
        }
        protected abstract void OnClosing(OrnamentWebSocket oWebSocket, HttpContext http);
        protected abstract void OnConnecting(OrnamentWebSocket socket, HttpContext http);
    }
}