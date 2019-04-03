using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace socket_server
{
    class AsyncObject
    {

        public Socket workingSocket = null;
        public const int bufferSize = 2048;
        public byte[] buffer = new byte[bufferSize];
        public StringBuilder sb = new StringBuilder();
        public void ClearBuffer()
        {
            Array.Clear(buffer, 0, bufferSize);
        }
    }
}
