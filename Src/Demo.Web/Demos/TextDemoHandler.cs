using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ornament.WebSockets;
using Ornament.WebSockets.Handlers;
using System.Net.WebSockets;
namespace Demo.Web.Demos
{
    public class TextDemoHandler : TextHandler
    {
        protected TextDemoHandler(IOptions<WebSocketOptions> options) : base(options)
        {
        }

        protected override void OnClosing(OrnamentWebSocket oWebSocket, HttpContext http)
        {
            GetClients()
                       .SendTextAsnyc(oWebSocket.Id + " logout, clients is：" + this.CountClients(),
                           CancellationToken.None);
        }

        protected override void OnConnecting(OrnamentWebSocket websocket, HttpContext http)
        {
            websocket.SendTextAsnyc("logon,online users is:" + this.CountClients());
            websocket.SendTextAsnyc("Your Id is :" + websocket.Id + " protocol :" + websocket.SubProtocol);

        }

        protected override void OnReceived(OrnamentWebSocket socket, HttpContext http, string data)
        {
            if (data == "error")
                throw new Exception("server exception for test");
            if (data.Length > 4096)
                socket.SendTextAsnyc("system receive data's length:" + data.Length);
            else
                socket.SendTextAsnyc("System received：" + data);

        }
    }
}
