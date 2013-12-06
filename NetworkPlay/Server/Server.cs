using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Objects;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Server
{
    class Server
    {
        Dictionary<Guid, Toon> toons;

        string networkBroadcast;

        public Server()
        {
            toons = new Dictionary<Guid, Toon>();
        }

        public void Run()
        {
            GetNetworkIp();
            new Thread(new ThreadStart(Listen)).Start();

            Send();

            Console.ReadLine();
        }

        public void Send()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint sendToPoint = new IPEndPoint(IPAddress.Parse("192.168.0.255"), 41000);

            BinaryFormatter bf = new BinaryFormatter();

            while (true)
            {
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, toons);

                socket.SendTo(ms.ToArray(), sendToPoint);

                ms.Close();
                Console.WriteLine(toons.Count);
                Thread.Sleep(10);
            }

            
        }

        public void Listen()
        {
            UdpClient udp = new UdpClient(42000);
            IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, 42000);

            while (true)
            {
                Toon givenToon;
                using (MemoryStream ms = new MemoryStream(udp.Receive(ref groupEp)))
                {
                    givenToon = (Toon)(new BinaryFormatter().Deserialize(ms));
                }

                //if this is a new player
                //else update the toon with what he said he is
                if (!toons.ContainsKey(givenToon.id))
                {
                    toons.Add(givenToon.id, givenToon);
                }
                else
                {
                    toons[givenToon.id] = givenToon;
                }
            }
        }

        public void GetNetworkIp()
        {
            Console.WriteLine("Please enter the network's broadcast IP: ");
            networkBroadcast = Console.ReadLine();

            //if it's not a valid IP then kill the process
            try
            {
                IPAddress.Parse(networkBroadcast);
            }
            catch (Exception e)
            {
                Console.WriteLine("You entered the wrong IP and you should feel bad");
                Thread.Sleep(2000);
                Thread.CurrentThread.Abort();
            }
        }

        static void Main(string[] args)
        {
            new Server().Run();
        }
    }
}
