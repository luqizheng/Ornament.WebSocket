using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ornament.WebSockets.WebSocketProtocols;

namespace Ornament.WebSockets
{
    public class OrnamentWebSocket
    {
        private readonly WebSocket _socket;
        private readonly int _bufferLenght = 4096;

        public OrnamentWebSocket(WebSocket socket, int bufferLength = 4096)
        {
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            _socket = socket;
            Id = Guid.NewGuid().ToString("N");
            _bufferLenght = bufferLength;
        }


        public string Id { get; }
        public IWebSocketProtocol Protocol { get; set; }

        public Task SendByText<TObject>(TObject t)
        {
            if (t == null)
                throw new ArgumentNullException(nameof(t));
            if (Protocol == null)
                throw new ArgumentException("Do not found Protocol for this WebSocket connection");
            var bytes = Protocol.ToObject(t);
            return SendAsnyc(bytes, _bufferLenght, WebSocketMessageType.Text, CancellationToken.None);
        }

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
            return Task.Run(async () =>
            {
                var splitInfo = Split(maxLength, bytes.Length);
                for (var index = 0; index < splitInfo.Length; index++)
                {
                    var info = splitInfo[index];
                    var arraySegment = new ArraySegment<byte>(bytes, info.Start, info.Length);
                    var isEnd = index >= splitInfo.Length;
                    await
                        _socket.SendAsync(
                            arraySegment,
                            messageType,
                            isEnd,
                            cancellationToken);
                }
            }, cancellationToken);
        }

        //public Task SendBinAsync(Stream stream, CancellationToken cancellationToken)
        //{
        //    return Task.Run(() =>
        //    {
        //        var bytes = new byte[bufferLenght];
        //        Task task = null;
        //        while (stream.Read(bytes, 0, bufferLenght) == bufferLenght)
        //        {
        //            task = _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, false,
        //                cancellationToken);
        //            task.Wait(cancellationToken);
        //        }
        //        task = _socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Binary, true,
        //            cancellationToken);
        //        task.Wait(cancellationToken);
        //    }, cancellationToken);
        //}


        private static SplitInfo[] Split(int maxLength, int totalLength)
        {
            var length = Convert.ToInt32(totalLength / maxLength);

            var result = new SplitInfo[length + 1];
            var step = 0;
            for (step = 0; step < length; step++)
                result[step] = new SplitInfo
                {
                    Length = maxLength,
                    Start = step * maxLength
                };


            var remind = totalLength % maxLength;
            if (remind != 0)
                result[step] = new SplitInfo
                {
                    Start = totalLength - maxLength * length,
                    Length = remind
                };
            return result;
        }


        private struct SplitInfo
        {
            public int Start { get; set; }
            public int Length { get; set; }
        }
    }
}