using System;

namespace Ornament.WebSockets.Collections
{
    /// <summary>
    ///     用于内置分组
    /// </summary>
    public class WebSocketGroupColllection : AbstractWebSocketDictionary<string>
    {
        /// <summary>
        /// </summary>
        /// <param name="websocket"></param>
        /// <param name="group"></param>
        public void Add(OrnamentWebSocket websocket, string group)
        {
            if (websocket == null) throw new ArgumentNullException(nameof(websocket));
            if (@group == null) throw new ArgumentNullException(nameof(@group));
            websocket.Group = group;
            AddIn(websocket, group);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="websocket"></param>
        public void Remove(OrnamentWebSocket websocket)
        {
            if (websocket == null) throw new ArgumentNullException(nameof(websocket));
            Remove(websocket, websocket.Group);
            websocket.Group = null;
        }
    }
}