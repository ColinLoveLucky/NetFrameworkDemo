using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitDemo
{
    public interface IFake
    {
        bool DoSomething(string actionname);
    }
    public class TestMock
    {
        [Fact]
        public void Test_Interface_IFake()
        {
            var mo = new Mock<IFake>();
            mo.Setup(f => f.DoSomething("Ping")).Returns(true);
            mo.Setup(f => f.DoSomething("PingF")).Returns(false);
            Assert.Equal(true, mo.Object.DoSomething("Ping"));
            Assert.Equal(false, mo.Object.DoSomething("PingF"));
        }
    }
}
