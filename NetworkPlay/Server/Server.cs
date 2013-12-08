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
        private int port;
        private List<NetworkStream> streams;
        private TcpListener listener;

        public Server()
        {
            port = 41337;
            streams = new List<NetworkStream>();
            listener = new TcpListener(port);

            new Thread(ListenForNewDudes).Start();
        }

        public void ListenForNewDudes()
        {
            listener.Start();

            while (true)
            {
                NetworkStream stream = listener.AcceptTcpClient().GetStream();
                streams.Add(stream);

                new Thread(() => Listen(stream)).Start();
            }
        }

        public void Listen(NetworkStream stream)
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

                    Console.WriteLine("received event: " + (Vector2)leEvent.value);
                    Console.WriteLine("from: " + leEvent.id);

                    if (leEvent.type == 0)
                    {
                        Vector2 thePosition = (Vector2)leEvent.value;
                        Console.WriteLine(thePosition);
                    }

                    Send(leEvent);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Server=>Listen(NetworkStream)");
            }
        }

        public void Send(Event leEvent)
        {
            foreach (NetworkStream stream in streams)
            {
                try
                {
                    MemoryStream ms = new MemoryStream();
                    new BinaryFormatter().Serialize(ms, leEvent);
                    stream.Write(ms.ToArray(), 0, ms.ToArray().Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine("something died :( Server=>Send(Event)");
                }
            }
        }

        static void Main(string[] args)
        {
            new Server();
        }
    }
}
