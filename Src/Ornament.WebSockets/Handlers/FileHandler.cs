using System;
using System.IO;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public class FileHandler : WebSocketHandler
    {
        private string _currentPath = null;
        private readonly string _folder;

        public FileHandler(string folder)
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));
            _folder = folder;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public Action<OrnamentWebSocket, HttpContext, WebSocketManager, FileInfo> OnReceived { get; set; }

        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http, byte[] bytes,
            WebSocketReceiveResult receiveResult,
            WebSocketManager manager)
        {
            var file = Path.Combine(_folder, _currentPath ?? (_currentPath = Guid.NewGuid().ToString("N")));
            using (var writer = File.Open(file + ".uploading", FileMode.Append))
            {
                writer.Write(bytes, 0, receiveResult.Count);
            }
            if (receiveResult.EndOfMessage)
            {
                File.Move(file + ".uploading", file + ".tmp");
                OnReceived?.Invoke(oWebSocket, http, manager, new FileInfo(file + ".tmp"));
            }

        }
    }
}