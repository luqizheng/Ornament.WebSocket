dotnet pack src/Ornament.WebSockets -c:release -o:./
copy *.nupkg c:\inetpub\wwwroot\packages /y