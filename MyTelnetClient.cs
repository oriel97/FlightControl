using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightSimulator
{
    /**
     * MyTelnetClient - the class that connecting,writing,reading and
     * disconnecting from the server. 
     */
    class MyTelnetClient : ITelnetClient
    {
        private TcpClient tcp;
        private NetworkStream networkStream;


        public MyTelnetClient()
        {
            this.tcp = new TcpClient();
        }
        public TcpClient getTCP()
        {
            return this.tcp;
        }
        // check if the tcp is null or not.
        public int getTcp()
        {
            if (this.tcp != null)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }
        // cnecting to the server.
        public void connect(string ip, int port)
        {
            IPAddress ipAdress = IPAddress.Parse(ip);
            IPEndPoint iPEndPoint = new IPEndPoint(ipAdress, port);
            if (tcp != null)
            {
                tcp.Connect(iPEndPoint);
                networkStream = tcp.GetStream();
            }
            tcp.ReceiveTimeout = 10000;
        }
        // check if we are connected.
        public bool isConnected()
        {
            return tcp != null && tcp.Connected;
        }
        // writing
        public void write(string command)
        {

            byte[] bytes = Encoding.ASCII.GetBytes(command);
            this.networkStream.Write(bytes, 0, bytes.Length);
        }
        //reading
        public string read()
        {
            byte[] bytes = new byte[2048];
            this.networkStream.Read(bytes, 0, 2048);
            return Encoding.ASCII.GetString(bytes);
        }
        // disconnect from the sever.
        public void disconnect()
        {
            if (tcp == null)
            {
                return;
            }
            tcp.Close();
            tcp = null;
        }
    }
}