using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;



// adapted from MSDN
// https://msdn.microsoft.com/en-us/library/fx6588te(v=vs.110).aspx

namespace AudioLoopbackStreamer
{
    class TcpStream : Stream
    {
        private Socket _clientSock = null;

        public void Listen(int portNumber=7070)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, portNumber);
            Socket _servSocket = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp);
     
            _servSocket.Bind(localEndPoint);
            _servSocket.Listen(1);

            _servSocket.BeginAccept(new AsyncCallback(AcceptCallback), _servSocket);
            Console.WriteLine("server listening.");
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            if (_clientSock != null)
                throw new Exception("dunno what's going on here tbh.");

            _clientSock = ((Socket)ar.AsyncState).EndAccept(ar);
            // _clientSock.Receive(new byte[1024]);
            Console.WriteLine("client connected.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            // write the buffer to TCP
            if (_clientSock == null)
            {
                // if not connected, who cares
                return;
            }
            try
            {
                _clientSock.Send(buffer, offset, count, SocketFlags.None);
            }
            catch (SocketException)
            {
                _clientSock.Dispose();
                _clientSock = null;
                Console.WriteLine("client disconnected.");
            }
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Flush()
        {
            //you might need to implement this
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get { throw new NotImplementedException(); }
        }

        public override long Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }

}
