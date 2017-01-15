using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    /// <summary>
    /// </summary>
    public abstract class BinaryHandler : WebSocketHandler
    {
        /// <summary>
        /// </summary>
        /// <param name="maxBuffer"></param>
        protected BinaryHandler(int maxBuffer) : base(maxBuffer)
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
                    if (oWebSocket.Buffer.Count + content.Length > BuffSize)
                        throw new BufferOverflowException(BuffSize);
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