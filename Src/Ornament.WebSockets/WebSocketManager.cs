using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using Ornament.WebSockets.WebSocketProtocols;

namespace Ornament.WebSockets
{
    public class WebSocketManager
    {
        private readonly ConcurrentDictionary<string, OrnamentWebSocket> _pools =
            new ConcurrentDictionary<string, OrnamentWebSocket>();


        internal OrnamentWebSocket AddNewSocket(WebSocket socket, IWebSocketProtocol protocol)
        {
            var result = new OrnamentWebSocket(socket) {Protocol = protocol};
            _pools.TryAdd(result.Id, result);
            return result;
        }


        public IEnumerable<OrnamentWebSocket> GetClients()
        {
            return _pools.Values;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">id 不能为空</exception>
        public OrnamentWebSocket Get(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            if (_pools.ContainsKey(id))
                return _pools[id];
            throw new ArgumentOutOfRangeException(nameof(id));
        }


        internal void Remove(string id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            OrnamentWebSocket closingOrnamentWebOrnamentWebSocket;
            _pools.TryRemove(id, out closingOrnamentWebOrnamentWebSocket);
        }
    }
}