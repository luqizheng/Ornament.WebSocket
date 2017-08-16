using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Ornament.WebSockets.Handlers
{
    public class FileHandler : WebSocketHandler
    {
        private readonly string _folder;
        private string _currentPath;

        /// <summary>
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="bufferSize"></param>
        /// <exception cref="DirectoryNotFoundException">folder not be found</exception>
        /// <exception cref="ArgumentNullException">folder is empty</exception>
        public FileHandler(string folder, IOptions<WebSocketOptions> options) : base(options)
        {
            if (string.IsNullOrEmpty(folder)) throw new ArgumentNullException(nameof(folder));

            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException(folder + " not found");

            _folder = folder;
        }

        public Action<OrnamentWebSocket, HttpContext, WebSocketHandler, FileInfo> OnReceived { get; set; }

        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult,
            WebSocketHandler handler)
        {
            var file = Path.Combine(_folder, _currentPath ?? (_currentPath = Guid.NewGuid().ToString("N")));
            using (var writer = File.Open(file + ".uploading", FileMode.Append))
            {
                writer.Write(bytes, 0, receiveResult.Count);
            }
            if (receiveResult.EndOfMessage)
            {
                File.Move(file + ".uploading", file + ".tmp");
                OnReceived?.Invoke(oWebSocket, http, handler, new FileInfo(file + ".tmp"));
            }
        }

        protected override void OnClosing(OrnamentWebSocket oWebSocket, HttpContext http)
        {
        }

        protected override void OnConnecting(OrnamentWebSocket socket, HttpContext http)
        {
        }
    }
}