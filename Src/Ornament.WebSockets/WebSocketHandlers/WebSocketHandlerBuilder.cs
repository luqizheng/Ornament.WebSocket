using System;
using Ornament.WebSockets.WebSocketProtocols;

namespace Ornament.WebSockets.WebSocketHandlers
{
    /// <summary>
    ///     setting WebSocket.
    /// </summary>
    public class WebSocketHandlerBuilder
    {
        private readonly WebSocketHandlerManager _manager;
        private readonly IServiceProvider _provider;


        internal WebSocketHandlerBuilder(IServiceProvider provider, WebSocketHandlerManager manager)
        {
#if DEBUG   
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (manager == null) throw new ArgumentNullException(nameof(manager));
#endif
            _provider = provider;
            _manager = manager;
        }

        public WebSocketHandler Add(string path, IWebSocketProtocol protocol)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var result = new WebSocketHandler(_provider,protocol);

            _manager.RegistHanler(path, result);
            return result;
        }
        public WebSocketHandler Add(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var result = new WebSocketHandler(_provider);

            _manager.RegistHanler(path, result);
            return result;
        }
    }
}