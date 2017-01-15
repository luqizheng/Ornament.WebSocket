using Ornament.WebSockets;
using Ornament.WebSockets.Handlers;
using Xunit;

namespace UnitTest
{
    public class WebSocketHandlerManagerTest
    {
        [Fact]
        public void TestRegist()
        {
            var manager = new WebSocketManager();

            var exprect = new TextHandler();
            manager.RegistHanler("test", exprect);

            Assert.True(manager.ContainsHandler("test"));

            var actual = manager.GetHandler("test");

            Assert.Equal(exprect, actual);
        }
    }
}