using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    public class OrnamentWebSocket
    {
        private readonly int _bufferLenght;
        private readonly WebSocket _socket;

        public OrnamentWebSocket(WebSocket socket, WebSocketHandler handler, int bufferLength = 4096)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            WebSocketHandler = handler;
            _socket = socket;
            Id = Guid.NewGuid().ToString("N");
            _bufferLenght = bufferLength;
        }

        internal string Group { get; set; }

        public WebSocketHandler WebSocketHandler { get; set; }


        public string Id { get; }

        public bool IsOpen => _socket.State == WebSocketState.Open;

        public string SubProtocol => _socket.SubProtocol;

        public Task SendTextAsnyc(string sendContent)
        {
            return SendTextAsnyc(sendContent, CancellationToken.None);
        }

        public Task SendTextAsnyc(string sendContent, CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(sendContent);
            return SendAsnyc(bytes, _bufferLenght, WebSocketMessageType.Text, cancellationToken);
        }

        public Task SendAsnyc(byte[] bytes, int maxLength, WebSocketMessageType messageType,
            CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                RunPart(maxLength, bytes.Length, (start, length, isLast) =>
                {
                    var arraySegment = new ArraySegment<byte>(bytes, start, length);

                    _socket.SendAsync(
                        arraySegment,
                        messageType,
                        isLast,
                        cancellationToken).Wait(cancellationToken);
                });
            }, cancellationToken);
        }

        public static void RunPart(int eachPartLength, int totalLength,
            Action<int, int, bool> action)
        {
            var length = Convert.ToInt32(totalLength/eachPartLength);
            var start = 0;
            var remind = totalLength%eachPartLength;
            for (var i = 0; i < length; i++)
            {
                start = i*eachPartLength;
                var isEnd = (remind == 0) && (i == length - 1);
                action(start, eachPartLength, isEnd);
            }

            if (remind != 0)
                action(start, remind, true);
        }
    }
}