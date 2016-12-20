namespace Ornament.WebSockets.Serializer
{
    public interface IWebSocketSerializer
    {
       
        byte[] ToObject<TObject>(TObject content);

        TObject GetObject<TObject>(byte[] bytes);
    }

}