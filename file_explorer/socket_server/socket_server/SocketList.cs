using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

namespace socket_server
{
    class SocketList
    {
        public struct ClientSocket // 각 client의 소켓과 client가 보고있는 경로를 저장 (폴더삭제, 이동 등의 상황에서 그 경로안을 보고 있는 사용자가 있다면 못하게 막아야하니까?)
        {
            public Socket clientSocket;
            public string clientPath;
        }
        public static List<ClientSocket> connectedClients = new List<ClientSocket>();

        public void setClient(Socket client,string clientPath)
        {
            ClientSocket clientSocket = new ClientSocket();
            clientSocket.clientSocket = client;
            clientSocket.clientPath = clientPath;
            connectedClients.Add(clientSocket);
        }

        public int getClient()
        {
            return connectedClients.Count;
        }
    }
}
