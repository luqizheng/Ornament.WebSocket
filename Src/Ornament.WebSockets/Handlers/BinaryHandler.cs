using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ornament.WebSockets.Handlers
{
    /// <summary>
    /// </summary>
    public abstract class BinaryHandler : WebSocketHandler
    {
        protected BinaryHandler(IOptions<WebSocketOptions> options) : base(options)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="http"></param>
        /// <param name="manager"></param>
        /// <param name="content"></param>
        protected abstract void ReceiveCompleteData(OrnamentWebSocket socket, HttpContext http, WebSocketHandler manager,
            byte[] content);

        /// <summary>
        /// </summary>
        /// <param name="oWebSocket"></param>
        /// <param name="http"></param>
        /// <param name="content"></param>
        /// <param name="receiveResult"></param>
        /// <param name="handler"></param>
        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] content,
            WebSocketReceiveResult receiveResult,
            WebSocketHandler handler)
        {
            if (receiveResult.EndOfMessage)
            {
                if (oWebSocket.Buffer.Any())
                {
                    if (oWebSocket.Buffer.Count + content.Length > Options.ReceiveBufferSize)
                        throw new BufferOverflowException(Options.ReceiveBufferSize);
                    oWebSocket.Buffer.AddRange(content);

                    content = oWebSocket.Buffer.ToArray();
                    oWebSocket.Buffer.Clear();
                }
                ReceiveCompleteData(oWebSocket, http, handler, content);
            }
            else
            {
                oWebSocket.Buffer.AddRange(content);
            }
        }
    }
}