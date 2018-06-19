using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitDemo.Socket
{
    public class WebHttpServer
    {
        public void ListenerSocker()
        {
            IPAddress localAddress = IPAddress.Loopback;

            IPEndPoint endPoint = new IPEndPoint(localAddress, 49155);


        }
    }
}
