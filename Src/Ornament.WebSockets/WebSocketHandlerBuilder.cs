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
        private readonly OrnamentWebSocketManager _manager;

        /// <summary>
        /// </summary>
        /// <param name="manager"></param>
        internal WebSocketHandlerBuilder()
        {

            _manager = OrnamentWebSocketManager.Instance;
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
        ///     纯文本交换数据
        /// </summary>
        /// <param name="matcher"></param>
        /// <returns></returns>
        public TextHandler AddText(Func<string, bool> matcher)
        {
            var result = new TextHandler(BuffeSize);

            _manager.RegistHanler(matcher, result);
            return result;
        }

        public T Add<T>(Func<string, bool> matcher, T handler)
            where T : WebSocketHandler
        {
            _manager.RegistHanler(matcher, handler);
            return handler;
        }

        public T Add<T>(string path, T handler)
            where T : WebSocketHandler
        {
            _manager.RegistHanler(path, handler);
            return handler;
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public FileHandler AddFileUploader(string path, string folder)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));
            var uploadFileFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
            var result = new FileHandler(uploadFileFolder, BuffeSize);
            _manager.RegistHanler(path, result);
            return result;
        }

        public FileHandler AddFileUploader(Func<string, bool> matcher, string folder)
        {
            var uploadFileFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
            var result = new FileHandler(uploadFileFolder, BuffeSize);
            _manager.RegistHanler(matcher, result);
            return result;
        }
    }
}