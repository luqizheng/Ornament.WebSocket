using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using Ornament.WebSockets.Handlers;

namespace Demo.Web.Demos
{
    public class UploadfileDemoHandler : FileHandler
    {
        public UploadfileDemoHandler(IOptions<WebSocketOptions> options) : base("uploadfiles", options)
        {
        }
    }
}
