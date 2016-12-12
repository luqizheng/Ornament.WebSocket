using System.Net.WebSockets;

namespace Ornament.WebSockets.WebSocketProtocols
{
    public interface IWebSocketProtocol
    {
        WebSocketMessageType MessageType { get; }
        byte[] ToObject<TObject>(TObject content);

        TObject GetObject<TObject>(byte[] bytes);
    }

}