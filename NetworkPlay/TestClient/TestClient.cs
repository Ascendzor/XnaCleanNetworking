using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkCommsDotNet;

namespace TestClient
{
    class TestClient
    {
        public void Run()
        {
            NetworkComms.DefaultListenPort = 41337;
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", handle);
            UDPConnection.StartListening(true);
        }

        public void handle(PacketHeader header, Connection conn, string myObj)
        {
            Console.WriteLine("Received: " + myObj);
        }

        static void Main(string[] args)
        {
            new TestClient().Run();
        }
    }
}
