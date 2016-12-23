using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public abstract class BinaryHandler : WebSocketHandler
    {
        private readonly List<byte> _buffer;


        protected BinaryHandler(int maxBuffer) : base(maxBuffer)
        {
            _buffer = new List<byte>(maxBuffer);
        }

        protected abstract void ReceiveCompleteData(OrnamentWebSocket socket, HttpContext http, WebSocketHandler manager,
            byte[] content);


        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] content,
            WebSocketReceiveResult receiveResult,
            WebSocketHandler handler)
        {
            if (receiveResult.EndOfMessage)
            {
                if (_buffer.Any())
                {
                    if (_buffer.Count + content.Length > BuffSize)
                        throw new BufferOverflowException(BuffSize);
                    _buffer.AddRange(content);

                    content = _buffer.ToArray();
                    _buffer.Clear();
                }
                ReceiveCompleteData(oWebSocket, http, handler, content);
            }
            else
            {
                _buffer.AddRange(content);
            }
        }
    }
}