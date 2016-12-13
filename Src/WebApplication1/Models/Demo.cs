using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ornament.WebSockets;

namespace WebApplication1.Models
{
    public static class Demo
    {
        public static WebSocketHandlerBuilder Demo1(this WebSocketHandlerBuilder setting)
        {
            var handler = setting.AddText("/repeat");
            handler.OnConnecting = (websocket, http, manager) =>
            {
                websocket.SendTextAsnyc("logon,online users is:" + manager.CountClients());
                websocket.SendTextAsnyc("Your Id is :" + websocket.Id +" protocol :"+websocket.SubProtocol);
            };
            handler.OnReceived = (websocket, http, text, manager) =>
            {
                if (text == "error")
                {
                    throw new Exception("server exception for test");
                }
                if (text.Length > 4096)
                {
                    websocket.SendTextAsnyc("system receive data's length:" + text.Length);
                }
                else
                {
                    websocket.SendTextAsnyc("System received：" + text);

                }
            };
            handler.OnClosed = (oWebSocket, http, manager) =>
            {
                manager.GetClients().SendTextAsnyc(oWebSocket.Id + " logout, clients is：" + manager.CountClients(), CancellationToken.None);
            };

            return setting;
        }

        public static WebSocketHandlerBuilder FileUploadDemo(this WebSocketHandlerBuilder setting)
        {
            var handler = setting.AddBinary("/uploadFile", "uploadfiles");

            handler.OnConnecting = (websocket, http, manager) =>
            {
                websocket.SendTextAsnyc("logon,online users is:" + manager.CountClients());
                websocket.SendTextAsnyc("Your Id is :" + websocket.Id);
            };
            handler.OnReceived = (websocket, http, manager, fileinfo) =>
            {
                try
                {
                    var length = fileinfo.Length;
                    var filename = fileinfo.Name;
                    websocket.SendTextAsnyc("system receive file's length:" + length + " name:" + filename);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            };
            handler.OnClosed = (oWebSocket, http, manager) =>
            {
                manager.GetClients().SendTextAsnyc(oWebSocket.Id + " logout, clients is：" + manager.CountClients(), CancellationToken.None);
            };

            return setting;
        }
    }
}
