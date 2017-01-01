namespace Ornament.WebSockets.Collections
{
    /// <summary>
    ///     用于内置分组
    /// </summary>
    public class WebSocketGroupColllection : AbstractWebSocketDictionary<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="websocket"></param>
        /// <param name="group"></param>
        public void Add(OrnamentWebSocket websocket, string group)
        {
            websocket.Group = group;
            base.AddIn(websocket, group);
        }

        public void Remove(OrnamentWebSocket websocket)
        {
            Remove(websocket, websocket.Group);
            websocket.Group = null;
        }
    }
}