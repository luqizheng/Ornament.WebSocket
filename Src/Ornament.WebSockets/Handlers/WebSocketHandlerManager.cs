using System;
using System.Collections.Generic;

namespace Ornament.WebSockets.Handlers
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

        public void RegistHanler(string path, WebSocketHandler handler)
        {
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            _pathHandlersMappings.Add(path.ToUpper(), handler);
            handler.WebSocketManager = _manager;
        }

        public WebSocketHandler GetHandler(string paht)
        {
            return _pathHandlersMappings[paht.ToUpper()]
                ;
        }

        public bool ContainsHandler(string path)
        {
            return _pathHandlersMappings.ContainsKey(path.ToUpper());
        }
    }
}