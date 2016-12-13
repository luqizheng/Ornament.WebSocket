dotnet build src/Ornament.WebSockets -o ./Bin/net461 -c:Release --framework:net461 --no-incremental --no-dependencies
dotnet build src/Ornament.WebSockets -o ./Bin/netstandard1.6 -c:Release --framework:netstandard1.6 --no-incremental  --no-dependencies

dotnet pack src/Ornament.WebSockets -c:release -o:./
copy *.nupkg c:\inetpub\wwwroot\packages /y