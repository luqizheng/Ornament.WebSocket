﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Ornament.WebSockets.Collections;

namespace Ornament.WebSockets.Handlers
{
    public abstract class WebSocketHandler
    {
        private readonly WebSocketCollection<string> _webSockets = new WebSocketCollection<string>();
        public Action<OrnamentWebSocket, HttpContext, WebSocketHandler> OnClosed;
        public Action<OrnamentWebSocket, HttpContext, WebSocketHandler> OnConnecting;

        protected WebSocketHandler(int buffSize = 4096)
        {
            BuffSize = buffSize;
        }


        public string Path { get; internal set; }
        public int BuffSize { get; }
        public WebSocketManager WebSocketManager { get; internal set; }

        private OrnamentWebSocket Add(WebSocket socket)
        {
            var result = new OrnamentWebSocket(socket, this);
            _webSockets.Add(result.Id, result);
            return result;
        }

        internal async Task Attach(HttpContext http, WebSocket socket)
        {
            var oWebSocket = Add(socket);

            OnConnecting?.Invoke(oWebSocket, http, this);
            try
            {
                while (socket.State == WebSocketState.Open)
                {
                    var buffer = new ArraySegment<byte>(new byte[BuffSize]);
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
                OnClosed?.Invoke(oWebSocket, http, this);
            }
        }


        protected abstract void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult, WebSocketHandler handler);

        public int CountClients()
        {
            return _webSockets.CountClients();
        }

        public IEnumerable<OrnamentWebSocket> GetClients()
        {
            return _webSockets.GetClients();
        }

        public bool TryGetWebSocket(string id, out OrnamentWebSocket weboSocket)
        {
            weboSocket = _webSockets.Get(id);
            return weboSocket != null;
        }
    }
}