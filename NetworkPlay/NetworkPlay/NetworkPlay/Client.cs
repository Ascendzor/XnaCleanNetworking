using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.Xna.Framework;
using System.IO;

namespace NetworkPlay
{
    class Client
    {
        private Toon me;
        private Toon him;

        private Socket socket;
        private IPAddress sendToIp;
        private IPEndPoint sendToPoint;

        public Client(Toon me, Toon him)
        {
            this.me = me;
            this.him = him;

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sendToIp = IPAddress.Parse("192.168.0.255");
            sendToPoint = new IPEndPoint(sendToIp, 41338);

            new Thread(new ThreadStart(listener)).Start();
        }

        public void sendData()
        {
            MemoryStream binaryBuffer = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(binaryBuffer))
            {
                bw.Write(me.GetPosition().X);
                bw.Write(me.GetPosition().Y);
            }

            byte[] messageBuffer = binaryBuffer.ToArray();

            socket.SendTo(messageBuffer, sendToPoint);
        }

        public void listener()
        {
            UdpClient udp = new UdpClient(41337);
            IPEndPoint groupEp = new IPEndPoint(IPAddress.Any, 41337);
            while (true)
            {
                byte[] receivedBytes = udp.Receive(ref groupEp);

                byte[] xValue = new byte[4];
                Buffer.BlockCopy(receivedBytes, 0, xValue, 0, 4);
                byte[] yValue = new byte[4];
                Buffer.BlockCopy(receivedBytes, 4, yValue, 0, 4);

                him.SetPosition(new Vector2(BitConverter.ToSingle(xValue, 0), BitConverter.ToSingle(yValue, 0)));
            }
        }
    }
}
