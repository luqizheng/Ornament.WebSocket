using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ornament.WebSockets.Collections
{
    public class WebSocketCollection<T>
    {
        /// <summary>
        ///     Key is id, values is ornamentWEbSockets
        /// </summary>
        private readonly ConcurrentDictionary<T, OrnamentWebSocket> _pools =
            new ConcurrentDictionary<T, OrnamentWebSocket>();


        public IEnumerable<OrnamentWebSocket> GetClients()
        {
            return _pools.Values;
        }

        public void Add(T id, OrnamentWebSocket webSocket)
        {
            _pools.TryAdd(id, webSocket);
        }

        /// <summary>
        /// </summary>
        /// <param name="id">socketId</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id 不能为空</exception>
        public OrnamentWebSocket Get(T id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (_pools.ContainsKey(id))
                return _pools[id];
            return null;
        }


        public void Remove(T id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            OrnamentWebSocket closingOrnamentWebOrnamentWebSocket;
            _pools.TryRemove(id, out closingOrnamentWebOrnamentWebSocket);
        }

        public int CountClients()
        {
            return _pools.Count;
        }
    }
}