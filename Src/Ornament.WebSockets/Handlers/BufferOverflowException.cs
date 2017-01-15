using System;

namespace Ornament.WebSockets.Handlers
{
    public class BufferOverflowException : Exception
    {
        public BufferOverflowException(int maxLength)
            : base($"actual length geater than" + maxLength)
        {
        }
    }
}