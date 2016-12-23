using System.Collections.Concurrent;

namespace Ornament.WebSockets.Collections
{
    public class GroupWebSocketCollection
    {
        private readonly ConcurrentDictionary<string, WebSocketCollection<string>> _groupSocketIdMapping =
            new ConcurrentDictionary<string, WebSocketCollection<string>>();


        public void Add(OrnamentWebSocket webSocket, string group = "default")
        {
            var list = _groupSocketIdMapping.GetOrAdd(group, s => new WebSocketCollection<string>());
            webSocket.Group = group;
            list.Add(webSocket.Id, webSocket);
        }

        public void Remove(OrnamentWebSocket socket)
        {
            WebSocketCollection<string> group;
            if (_groupSocketIdMapping.TryGetValue(socket.Group, out group))
                group.Remove(socket.Id);
        }
    }
}