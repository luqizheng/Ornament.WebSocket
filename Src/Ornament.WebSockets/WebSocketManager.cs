using System;
using System.Collections.Generic;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    public class WebSocketManager
    {
        private readonly Dictionary<string, WebSocketHandler> _pathHandlersMappings =
            new Dictionary<string, WebSocketHandler>();

        public void RegistHanler(string path, WebSocketHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            handler.WebSocketManager = this;
            handler.Path = path;
            lock (_pathHandlersMappings)
            {
                _pathHandlersMappings.Add(path.ToUpper(), handler);
            }
        }

        public WebSocketHandler GetHandler(string path)
        {
            return _pathHandlersMappings[path.ToUpper()];
        }

        public bool ContainsHandler(string path)
        {
            return _pathHandlersMappings.ContainsKey(path.ToUpper());
        }

        public OrnamentWebSocket GetWebSocket(string id)
        {
            foreach (var handler in _pathHandlersMappings.Values)
            {
                OrnamentWebSocket webSocket;
                if (handler.TryGetWebSocket(id, out webSocket))
                    return webSocket;
            }
            throw new OranmentWebSocketException("Can not found id=" + id + " websocket");
        }

        public OrnamentWebSocket GetWebSocket(string id, string path)
        {
            WebSocketHandler handler;

            if (_pathHandlersMappings.TryGetValue(path, out handler))
            {
                OrnamentWebSocket webSocket;
                if (handler.TryGetWebSocket(id, out webSocket))
                    return webSocket;
            }
            throw new OranmentWebSocketException("Can not found id=" + id + " websocket");
        }
    }
}