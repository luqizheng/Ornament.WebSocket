using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Ornament.WebSockets.Handlers;

namespace Ornament.WebSockets
{
    /// <summary>
    /// </summary>
    public class OrnamentWebSocketManager
    {

        private readonly Dictionary<Func<string, bool>, Type> _matcher
            = new Dictionary<Func<string, bool>, Type>();


        private readonly Dictionary<string, Type> _pathHandlersMappings =
            new Dictionary<string, Type>();


        public OrnamentWebSocketManager()
        {


        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handler"></param>
        public void RegistHanler<T>(string path)
            where T : WebSocketHandler
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            lock (_pathHandlersMappings)
            {
                _pathHandlersMappings.Add(path.ToUpper(), typeof(T));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handlerType"></param>
        public void RegistHanler(string path, Type handlerType)

        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            lock (_pathHandlersMappings)
            {
                _pathHandlersMappings.Add(path.ToUpper(), handlerType);
            }
        }


        public void RegistHanler<T>(Func<string, bool> matcher)
            where T : WebSocketHandler
        {
            if (matcher == null)
                throw new ArgumentNullException(nameof(matcher));
            lock (_matcher)
                _matcher.Add(matcher, typeof(T));
        }
        public void RegistHanler(Func<string, bool> matcher, Type type)

        {
            if (matcher == null)
                throw new ArgumentNullException(nameof(matcher));
            lock (_matcher)
                _matcher.Add(matcher, type);
        }

        /// <summary>
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="hanndler"></param>
        /// <returns></returns>
        public bool GetHandler(IServiceProvider provider, string path, out IWebSocketHandler hanndler)

        {

            if (path == null)
                throw new ArgumentNullException(nameof(path));
            hanndler = null;
            var handlerType = GetHandlerType(path);
            if (handlerType != null)
                hanndler = (IWebSocketHandler)provider.GetService(handlerType);
            return hanndler != null;
        }

        /// <summary>
        ///     冲注册的handler中获取handler的属性
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private Type GetHandlerType(string path)
        {
            Type resultType = null;
            if (_pathHandlersMappings.ContainsKey(path.ToUpper()))
                resultType = _pathHandlersMappings[path.ToUpper()];
            foreach (var matcher in _matcher)
                if (matcher.Key(path))
                    resultType = matcher.Value;
            return resultType;
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="OrnamentWebSocketException"></exception>
        public OrnamentWebSocket GetWebSocket(IServiceProvider provider, string id, string path)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            IWebSocketHandler handler;

            if (GetHandler(provider, path, out handler))
            {
                OrnamentWebSocket webSocket;
                if (handler.TryGetWebSocket(id, out webSocket))
                    return webSocket;
            }
            throw new OrnamentWebSocketException("Can not found id=" + id + " websocket");
        }
    }
}