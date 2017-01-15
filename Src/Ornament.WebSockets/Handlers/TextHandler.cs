using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public class TextHandler : WebSocketHandler
    {
        public Action<OrnamentWebSocket, HttpContext, string, WebSocketHandler> OnReceived;

        public TextHandler(int buffSize = 4096) : base(buffSize)
        {
        }

        public bool CallByCompleteMessage { get; set; } = true;

        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http,
            byte[] content, WebSocketReceiveResult receiveResult, WebSocketHandler manager)
        {
            if (receiveResult.EndOfMessage)
            {
                if (CallByCompleteMessage && oWebSocket.Buffer.Any())
                {
                    oWebSocket.Buffer.AddRange(content);

                    content = oWebSocket.Buffer.ToArray();
                    oWebSocket.Buffer.Clear();
                }

                var message = Encoding.UTF8.GetString(content, 0, receiveResult.Count);
                OnReceived?.Invoke(oWebSocket, http, message, manager);
            }
            else
            {
                oWebSocket.Buffer.AddRange(content);
            }
        }
    }
}