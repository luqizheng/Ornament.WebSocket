using System;
using System.IO;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    /// <summary>
    ///     setting WebSocket.
    /// </summary>
    public class WebSocketHandlerBuilder
    {
        private readonly WebSocketHandlerManager _manager;


        internal WebSocketHandlerBuilder(WebSocketHandlerManager manager)
        {
#if DEBUG

            if (manager == null) throw new ArgumentNullException(nameof(manager));
#endif

            _manager = manager;
        }

        public TextHandler AddText(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var result = new TextHandler();
            _manager.RegistHanler(path, result);
            return result;
        }

        public FileHandler AddBinary(string path, string folder)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var uploadFileFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
            var result = new FileHandler(uploadFileFolder);
            _manager.RegistHanler(path, result);
            return result;
        }
    }
}