using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public class FileHandler : WebSocketHandler
    {
        private readonly string _folder;
        private string _currentPath;

        public FileHandler(string folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            _folder = folder;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public Action<OrnamentWebSocket, HttpContext, WebSocketHandler, FileInfo> OnReceived { get; set; }

        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult,
            WebSocketHandler handler)
        {
            var file = System.IO.Path.Combine(_folder, _currentPath ?? (_currentPath = Guid.NewGuid().ToString("N")));
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
    }
}