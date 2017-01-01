using System;

namespace Ornament.WebSockets
{
    public class OrnamentWebSocketException : Exception
    {
        public OrnamentWebSocketException(string message) : base(message)
        {
        }
    }
}