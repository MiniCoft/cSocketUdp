using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net.Sockets;
using System.Net;

using System.Threading;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CSocket
{
    
    class cSocket
    {
        int mRecvBuffer = 20;
        string ip;
        int port;
        public cSocket(string _ip, int _port)
        {
            ip = _ip;
            port = _port;
        }
        public void Start()
        {
            Thread thread = new Thread(new ThreadStart(SocketListener));
            thread.Start();
            while (true)
            {
                string s = Console.ReadLine();
                if (s == "exit")
                    break;
                if (s.Length > mRecvBuffer)
                {
                    Console.WriteLine("This Sting is Over " + mRecvBuffer);
                    continue;
                }
                Send(s);
            }
            thread.Abort();
        }
        void SocketListener()
        {

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, port);
            Socket mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mSocket.Bind(endpoint);

            EndPoint Remote = (EndPoint)(endpoint);
            while (true)
            {
                int recv;
                byte[] bytes = new byte[mRecvBuffer];
                recv = mSocket.ReceiveFrom(bytes, ref Remote);

                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream stream = new MemoryStream(bytes);
                string s = System.Text.Encoding.Default.GetString(bytes);

                Console.WriteLine("Recv:" + s);

            }

        }


        public void Send(string msg)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            byte[] bytes = System.Text.Encoding.Default.GetBytes(msg);
            s.SendTo(bytes, bytes.Length, SocketFlags.None, endPoint);
        }
    }
}
