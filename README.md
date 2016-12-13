# Ornament.WebSocket
Wrap Dotnet core WebSocket let it easy to use. 


# Quick Start
add Oranment.WebSockets.dll
add following code Startup.cs
```
  public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrnamentWebSocket();
        }
        
         public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseOrnamentWebSocket(setting =>
            {
                 var handler = setting.AddText("/repeat");  // add text send and receive.
                 handler.OnConnecting = (websocket, http, manager) =>
                 {
                    //onConnecting.
                    websocket.SendTextAsnyc("logon,online users is:" + manager.CountClients());
                    websocket.SendTextAsnyc("Your Id is :" + websocket.Id);
                 };
                handler.OnReceived = (websocket, http, text, manager) =>
              {
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
                //oWebScoket's status in closed and manger do not include it.
                
                //send to another clients.
                manager.GetClients().SendTextAsnyc(oWebSocket.Id + " logout, clients is：" + manager.CountClients(), CancellationToken.None);
            };

            });
```
# Demos are inlcuded in src-code. 
1) QuickStart 
2) file upload.
