using System;
using System.Collections.Concurrent;

namespace Ornament.WebSockets.Collections
{
    /// <summary>
    /// </summary>
    public abstract class AbstractWebSocketDictionary<TKey>
    {
        private readonly ConcurrentDictionary<TKey, WebSocketCollection> _groupSocketIdMapping =
            new ConcurrentDictionary<TKey, WebSocketCollection>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webSocket"></param>
        /// <param name="groupKey"></param>
        protected virtual void AddIn(OrnamentWebSocket webSocket, TKey groupKey)
        {
            if (webSocket == null) throw new ArgumentNullException(nameof(webSocket));


            var list = _groupSocketIdMapping.GetOrAdd(groupKey, s => new WebSocketCollection());

            list.Add(webSocket);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="groupKey"></param>
        protected virtual void Remove(OrnamentWebSocket socket, TKey groupKey)
        {
            if (socket == null) throw new ArgumentNullException(nameof(socket));
            WebSocketCollection group;
            if (_groupSocketIdMapping.TryGetValue(groupKey, out group))
                group.Remove(socket.Id);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupKey"></param>
        /// <param name="sockets"></param>
        /// <returns></returns>
        public virtual bool TryGetGroup(TKey groupKey, out WebSocketCollection sockets)
        {
            return _groupSocketIdMapping.TryGetValue(groupKey, out sockets);
        }
    }
}