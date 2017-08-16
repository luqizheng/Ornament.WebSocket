using System;
using System.Collections.Generic;
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

        public OrnamentWebSocketManager(IServiceProvider provider)
        {
            Provider = provider;
        }

        private IServiceProvider Provider { get; }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="handler"></param>
        public void RegistHanler<T>(string path)
            where T : WebSocketHandler
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            //handler.Path = path;
            lock (_pathHandlersMappings)
            {
                _pathHandlersMappings.Add(path.ToUpper(), typeof(T));
            }
        }

        public void RegistHanler<T>(Func<string, bool> matcher)
            where T : WebSocketHandler
        {
            if (matcher == null)
                throw new ArgumentNullException(nameof(matcher));
            //handler.Path = path;
            _matcher.Add(matcher, typeof(T));
        }

        /// <summary>
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool GetHandler(string path, out IWebSocketHandler hanndler)

        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            hanndler = null;
            var handlerType = GetHandlerType(path);
            if (handlerType != null)
                hanndler = (IWebSocketHandler) Provider.GetService(handlerType);
            return handlerType != null;
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


        ///// <summary>
        ///// 根据id 获取 websocket
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        ///// <exception cref="OrnamentWebSocketException"></exception>
        //public OrnamentWebSocket GetWebSocket(string id)
        //{
        //    if (id == null)
        //    {
        //        throw new ArgumentNullException(nameof(id));
        //    }
        //    this.GetHandler()
        //    foreach (var handlerType in _pathHandlersMappings.Values)
        //    {
        //        OrnamentWebSocket webSocket;
        //        if (handlerType.TryGetWebSocket(id, out webSocket))
        //            return webSocket;
        //    }
        //    throw new OrnamentWebSocketException("Can not found id=" + id + " websocket");
        //}

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="OrnamentWebSocketException"></exception>
        public OrnamentWebSocket GetWebSocket(string id, string path)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            IWebSocketHandler handler;

            if (GetHandler(path, out handler))
            {
                OrnamentWebSocket webSocket;
                if (handler.TryGetWebSocket(id, out webSocket))
                    return webSocket;
            }
            throw new OrnamentWebSocketException("Can not found id=" + id + " websocket");
        }
    }
}