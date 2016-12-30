# Ornament.WebSocket
Wrap Dotnet core WebSocket let it easy to use. 

# install
nuget install Ornament.WebSocket

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
    app.UseWebSockets();
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
            websocket.SendTextAsnyc("system receive data's length:" + text.Length);
        };
        handler.OnClosed = (oWebSocket, http, manager) =>
        {
            manager.GetClients().SendTextAsnyc(oWebSocket.Id + " logout, clients is：" + manager.CountClients(), CancellationToken.None);
        };

});
```
# Get All WebSockets
```
  public class XXXController
  {
      WebSocketManager _manager;
      public XXXController(Ornament.WebSockets.WebSocketManager manager)
      {
          //from ioc 
          _manager=manager;
      }

      public void Brocadcast(string message)
      {

         _manager.GetHandler("/repeat").GetClients().SendTextAsnyc(message)
      }
  }
```
# Demo are inlcuded in src-code. 
* Text transfer 
* file upload.
