using System;
using System.IO;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    /// <summary>
    ///     setting WebSocket.
    /// </summary>
    //public class WebSocketHandlerBuilder
    //{
    //    private readonly OrnamentWebSocketManager _manager;

    //    /// <summary>
    //    /// </summary>
    //    /// <param name="manager"></param>
    //    public WebSocketHandlerBuilder(OrnamentWebSocketManager manager)
    //    {
    //        _manager = manager;
    //    }


    //    public void Add<T>(Func<string, bool> matcher, T handler)
    //        where T : WebSocketHandler
    //    {
    //        if (matcher == null)
    //            throw new ArgumentNullException(nameof(matcher));

    //        if (handler == null)
    //            throw new ArgumentNullException(nameof(handler));

    //        _manager.RegistHanler<T>(matcher);
    //    }

    //    public void Add<T>(string path)
    //        where T : WebSocketHandler
    //    {
    //        if (path == null)
    //            throw new ArgumentNullException(nameof(path));
    //        _manager.RegistHanler<T>(path);
    //    }

    ///// <summary>
    ///// </summary>
    ///// <param name="path"></param>
    ///// <param name="folder"></param>
    ///// <returns></returns>
    //public FileHandler AddFileUploader(string path, string folder)
    //{
    //    if (string.IsNullOrEmpty(path))
    //        throw new ArgumentNullException(nameof(path));
    //    var uploadFileFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
    //    var result = new FileHandler(uploadFileFolder, BuffeSize);
    //    _manager.RegistHanler(path, result);
    //    return result;
    //}

    //public FileHandler AddFileUploader(Func<string, bool> matcher, string folder)
    //{
    //    var uploadFileFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);
    //    var result = new FileHandler(uploadFileFolder, BuffeSize);
    //    _manager.RegistHanler(matcher, result);
    //    return result;
    //}
    //}
}