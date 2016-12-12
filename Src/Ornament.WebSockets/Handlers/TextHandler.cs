﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Ornament.WebSockets.Handlers
{
    public class TextHandler : WebSocketHandler
    {
        public List<byte> Buffer = new List<byte>();
        public Action<OrnamentWebSocket, HttpContext, string, WebSocketManager> OnReceived;
        public bool CallByCompleteMessage { get; set; } = true;

        protected override void OnReceivedData(OrnamentWebSocket oWebSocket, HttpContext http,
            byte[] content, WebSocketReceiveResult receiveResult, WebSocketManager manager)
        {
            if (receiveResult.EndOfMessage)
            {
                if (CallByCompleteMessage && Buffer.Any())
                {
                    Buffer.AddRange(content);

                    content = Buffer.ToArray();
                    this.Buffer.Clear();
                }

                var message = Encoding.UTF8.GetString(content, 0, receiveResult.Count);
                OnReceived?.Invoke(oWebSocket, http, message, manager);
            }
            else
            {
                Buffer.AddRange(content);
            }
        }
    }
}