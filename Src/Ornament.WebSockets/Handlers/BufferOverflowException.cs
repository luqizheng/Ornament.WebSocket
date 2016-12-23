using System;

namespace Ornament.WebSockets.Handlers
{
    public class BufferOverflowException : Exception
    {
        public BufferOverflowException(int maxLength)
            : base($"acutla length geater than" + maxLength)
        {
        }
    }
}