using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ornament.WebSockets.Handlers
{
    public abstract class TextHandler : WebSocketHandler
    {
  

        protected TextHandler(IOptions<WebSocketOptions> options) : base(options)
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
                OnReceived(oWebSocket, http, message);
            }
            else
            {
                oWebSocket.Buffer.AddRange(content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="http"></param>
        /// <param name="data"></param>
        /// 
        protected abstract void OnReceived(OrnamentWebSocket socket, HttpContext http, string data);

    }
}