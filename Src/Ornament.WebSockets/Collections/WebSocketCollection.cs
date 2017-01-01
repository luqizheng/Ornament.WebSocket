using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Ornament.WebSockets.Collections
{
    public class WebSocketCollection : WebSocketCollection<string>
    {
        protected override string GetKey(OrnamentWebSocket webSocket)
        {
            return webSocket.Id;
        }
    }

    public abstract class WebSocketCollection<T>
    {
        /// <summary>
        ///     Key is key, values is ornamentWEbSockets
        /// </summary>
        private readonly ConcurrentDictionary<T, OrnamentWebSocket> _pools =
            new ConcurrentDictionary<T, OrnamentWebSocket>();


        public IEnumerable<OrnamentWebSocket> GetClients()
        {
            return _pools.Values;
        }

        protected abstract T GetKey(OrnamentWebSocket webSocket);


        public virtual void Add(OrnamentWebSocket webSocket)
        {
            var id = GetKey(webSocket);
            _pools.TryAdd(id, webSocket);
        }

        /// <summary>
        /// </summary>
        /// <param name="key">socketId</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">key 不能为空</exception>
        public virtual OrnamentWebSocket Get(T key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (_pools.ContainsKey(key))
                return _pools[key];
            return null;
        }


        public virtual void Remove(T id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            OrnamentWebSocket closingOrnamentWebOrnamentWebSocket;
            _pools.TryRemove(id, out closingOrnamentWebOrnamentWebSocket);
        }

        public virtual bool IsEmpty()
        {
            return _pools.Count != 0;
        }

        public virtual int CountClients()
        {
            return _pools.Count;
        }
    }
}