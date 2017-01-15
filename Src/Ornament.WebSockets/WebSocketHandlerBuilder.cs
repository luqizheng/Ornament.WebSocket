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
        private readonly WebSocketManager _manager;

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        internal WebSocketHandlerBuilder(WebSocketManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));
            _manager = manager;
        }

        public int BuffeSize { get; set; } = 4096;

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">path can't be null</exception>
        public TextHandler AddText(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            var result = new TextHandler(BuffeSize);

            _manager.RegistHanler(path, result);
            return result;
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public FileHandler AddBinary(string path, string folder)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var uploadFileFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
            var result = new FileHandler(uploadFileFolder, BuffeSize);
            _manager.RegistHanler(path, result);
            return result;
        }
    }
}