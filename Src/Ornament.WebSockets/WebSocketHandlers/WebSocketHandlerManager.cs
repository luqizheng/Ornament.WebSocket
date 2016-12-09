using System;
using System.Collections.Generic;

namespace Ornament.WebSockets.WebSocketHandlers
{
    public class WebSocketHandlerManager
    {
        private readonly WebSocketManager _manager;

        private readonly Dictionary<string, WebSocketHandler> _pathHandlersMappings =
            new Dictionary<string, WebSocketHandler>();

        public WebSocketHandlerManager(WebSocketManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            _manager = manager;
        }

        internal void RegistHanler(string path, WebSocketHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            _pathHandlersMappings.Add(path, handler);
            handler.WebSocketManager = _manager;
        }

        internal WebSocketHandler GetHandler(string paht)
        {
            return _pathHandlersMappings[paht.ToUpper()]
                ;
        }

        internal bool ContainsHandler(string path)
        {
            return _pathHandlersMappings.ContainsKey(path.ToUpper());
        }
    }
}