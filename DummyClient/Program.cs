using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7000);

            Connector connector = new Connector();

            connector.Connect(endPoint, 
                () => { return SessionManager.Instance.Generate(); },
                99); // 테스트할 Client 개수

            while (true)
            {
                try
                {
                    SessionManager.Instance.SendForEach();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                Thread.Sleep(2000);
            }
        }
    }
}
 