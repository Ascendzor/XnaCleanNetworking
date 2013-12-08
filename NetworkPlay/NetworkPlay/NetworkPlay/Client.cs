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
        int port;
        NetworkStream stream;
        TcpClient client;
        IPAddress server;

        public Client()
        {
            port = 41337;
            new Thread(Connect).Start();
            Connect();

            new Thread(Listen).Start();
        }

        public void Listen()
        {
            int i;

            byte[] bytes = new byte[1024];

            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    byte[] arrBytes = bytes;
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(arrBytes, 0, arrBytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    Event leEvent = (Event)new BinaryFormatter().Deserialize(memStream);

                    if (!Game1.them.ContainsKey(leEvent.id))
                    {
                        Game1.them.Add(leEvent.id, new Toon((Vector2)leEvent.value));
                    }
                    else
                    {
                        Game1.them[leEvent.id].SetGoal((Vector2)leEvent.value);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Client=>Listen()");
            }
        }

        public void SubmitEvent(Event leEvent)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                new BinaryFormatter().Serialize(ms, leEvent);

                byte[] bytes = ms.ToArray();
                stream.Write(bytes, 0, bytes.Length);

                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Client=>SubmitEvent(Event)");
            }
        }

        public void Connect()
        {
            client = new TcpClient("192.168.1.123", port);

            stream = client.GetStream();
        }

        public void Disconnect()
        {
            stream.Close();
            client.Close();
        }
    }
}
