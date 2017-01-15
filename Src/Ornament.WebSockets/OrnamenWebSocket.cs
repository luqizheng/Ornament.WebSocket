using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    /// <summary>
    /// Websocket of wrapper
    /// </summary>
    public class OrnamentWebSocket
    {
        private readonly int _bufferLenght;
        private readonly WebSocket _socket;
        private string _group;
        internal List<byte> Buffer = new List<byte>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="handler"></param>
        /// <param name="bufferLength"></param>
        /// <exception cref="ArgumentNullException">socket or handler is null</exception>
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
        /// <summary>
        /// Gets the Group Name 
        /// </summary>
        public string Group
        {
            get { return _group; }
            internal set
            {
                if (_group == value)
                    throw new ArgumentException("value", $"WebSocket was belong to another group. can't be changed.");
                _group = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public WebSocketHandler WebSocketHandler { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsOpen => _socket.State == WebSocketState.Open;
        /// <summary>
        /// 
        /// </summary>
        public string SubProtocol => _socket.SubProtocol;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendContent"></param>
        /// <returns></returns>
        public Task SendTextAsnyc(string sendContent)
        {
            return SendTextAsnyc(sendContent, CancellationToken.None);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sendContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SendTextAsnyc(string sendContent, CancellationToken cancellationToken)
        {
            var bytes = Encoding.UTF8.GetBytes(sendContent);
            return SendAsnyc(bytes, _bufferLenght, WebSocketMessageType.Text, cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="maxLength"></param>
        /// <param name="messageType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eachPartLength"></param>
        /// <param name="totalLength"></param>
        /// <param name="action"></param>
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