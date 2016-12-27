using System;
using System.Collections.Concurrent;

namespace Ornament.WebSockets.Collections
{
    public class GroupWebSocketCollection
    {
        private readonly ConcurrentDictionary<string, WebSocketCollection<string>> _groupSocketIdMapping =
            new ConcurrentDictionary<string, WebSocketCollection<string>>();


        public void Add(OrnamentWebSocket webSocket, string group = "default")
        {
            if (webSocket == null) throw new ArgumentNullException(nameof(webSocket));
            if (group == null) throw new ArgumentNullException(nameof(group));
            webSocket.Group = group;
            var list = _groupSocketIdMapping.GetOrAdd(group, s => new WebSocketCollection<string>());

            list.Add(webSocket.Id, webSocket);
        }

        public void Remove(OrnamentWebSocket socket)
        {
            WebSocketCollection<string> group;
            if (_groupSocketIdMapping.TryGetValue(socket.Group, out group))
                group.Remove(socket.Id);
        }

        public bool TryGetGroup(string groupname, out WebSocketCollection<string> sockets)
        {
            if (groupname == null) throw new ArgumentNullException(nameof(groupname));
            return _groupSocketIdMapping.TryGetValue(groupname, out sockets);
        }
    }
}