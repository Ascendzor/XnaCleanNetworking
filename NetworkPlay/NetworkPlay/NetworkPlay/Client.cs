using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Objects;

namespace NetworkPlay
{
    class Client
    {
        private Toon me;

        public Client(Toon me)
        {
            this.me = me;

            new Thread(new ThreadStart(Send)).Start();
            
            new Thread(new ThreadStart(Listen)).Start();
        }

        public void Listen()
        {
            UdpClient udp = new UdpClient(41000);
            IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, 41000);

            while (true)
            {
                Dictionary<Guid, Toon> givenToons;
                using (MemoryStream ms = new MemoryStream(udp.Receive(ref groupEp)))
                {
                    givenToons = (Dictionary<Guid, Toon>)(new BinaryFormatter().Deserialize(ms));
                }

                Game1.them = givenToons;
            }
        }

        public void Send()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse("192.168.0.255"), 42000);
            while (true)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, me);

                socket.SendTo(ms.ToArray(), sendToPoint);

                ms.Close();
                Thread.Sleep(10);
            }
        }
    }
}
