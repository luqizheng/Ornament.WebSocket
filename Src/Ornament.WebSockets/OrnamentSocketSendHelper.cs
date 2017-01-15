using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Ornament.WebSockets
{
    public static class OrnamentSocketSendHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sockets"></param>
        /// <param name="bytes"></param>
        /// <param name="maxLength"></param>
        /// <param name="messageType"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task[] Send(this IEnumerable<OrnamentWebSocket> sockets,
            byte[] bytes, int
                maxLength, WebSocketMessageType messageType,
            CancellationToken cancellationToken)
        {
            return Run(sockets, socket => socket.SendAsnyc(bytes, maxLength, messageType, cancellationToken));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sockets"></param>
        /// <param name="sendContent"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task[] SendTextAsnyc(this IEnumerable<OrnamentWebSocket> sockets, string sendContent,
            CancellationToken cancellationToken)
        {
            return Run(sockets, socket => socket.SendTextAsnyc(sendContent, cancellationToken));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sockets"></param>
        /// <param name="sendContent"></param>
        /// <returns></returns>
        public static Task[] SendTextAsnyc(this IEnumerable<OrnamentWebSocket> sockets, string sendContent)
        {
            if (sockets == null) throw new ArgumentNullException(nameof(sockets));
            if (sendContent == null) throw new ArgumentNullException(nameof(sendContent));
            return Run(sockets, socket =>
            {
                if (socket.IsOpen)
                {
                    var task = socket.SendTextAsnyc(sendContent, CancellationToken.None);
                    return task;
                }
                return Task.FromResult(0);
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sockets"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        private static Task[] Run(IEnumerable<OrnamentWebSocket> sockets, Func<OrnamentWebSocket, Task> func)
        {
            var list = new List<Task>(sockets.Count());
            foreach (var socket in sockets)
            {
                var task = func(socket);
                list.Add(task);
            }
            return list.ToArray();
        }
    }
}