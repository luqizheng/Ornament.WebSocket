using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public abstract class BinaryHandler : WebSocketHandler
    {
        private readonly List<byte> _buffer;
        private readonly int _maxBuffer;


        protected BinaryHandler(int maxBuffer)
        {
            _maxBuffer = maxBuffer;

            _buffer = new List<byte>(_maxBuffer);
        }

        protected abstract void ReceiveCompleteData(OrnamentWebSocket socket, HttpContext http, WebSocketManager manager,
            byte[] content);


        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] content,
            WebSocketReceiveResult receiveResult,
            WebSocketManager manager)
        {
            if (receiveResult.EndOfMessage)
            {
                if (_buffer.Any())
                {
                    if (_buffer.Count + content.Length > _maxBuffer)
                        throw new BufferOverflowException(_maxBuffer);
                    _buffer.AddRange(content);

                    content = _buffer.ToArray();
                    _buffer.Clear();
                }
                ReceiveCompleteData(oWebSocket, http, manager, content);
            }
            else
            {
                _buffer.AddRange(content);
            }
        }
    }
}