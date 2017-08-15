using System;
using Ornament.WebSockets;
using Xunit;

namespace UnitTest
{
    public class OrnamentWebSocketTests
    {
        [Fact]
        public void OrnamentWebSocketSplit()
        {
            var each = 400;
            var total = 10550;

            var expectCount = Convert.ToInt32(total / each) + 1;
            var actualCount = 0;
            OrnamentWebSocket.RunPart(each, total, (start, length, isLast) =>
            {
                if (!isLast)
                {
                    Assert.Equal(actualCount * each, start);
                    Assert.Equal(length, each);
                }
                else
                {
                    Assert.Equal(total % each, length);
                    Assert.True(isLast);
                }
                actualCount++;
            });
        }
    }
}