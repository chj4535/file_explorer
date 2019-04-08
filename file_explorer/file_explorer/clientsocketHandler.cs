using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using System.Threading;

namespace file_explorer
{
    
    class ClientSocketHandler
    {
        static Socket mainSock;
        static CommandClassification cmdHandler = new CommandClassification();
        static ClientSocketHandler()
        {
            try
            {
                string address = "localhost"; // "127.0.0.1" 도 가능
                int port = 2233;
                mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP); // 소켓 초기화
                mainSock.Connect(address, port);
            }
            catch
            {
                MessageBox.Show("서버가 실행되지 않고 있습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                System.Environment.Exit(1);
            }
            Console.WriteLine("소켓연결 성공");
            AsyncObject ao = new AsyncObject();
            ao.workingSocket = mainSock;
            mainSock.BeginReceive(ao.buffer, 0, 2048, 0, DataReceived, ao);
        }

        static void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.

            AsyncObject obj = (AsyncObject)ar.AsyncState;
            Socket server = obj.workingSocket;
            // 데이터 수신을 끝낸다.
            int received;
            
            try
            {
                // 데이터 수신을 끝낸다.
                received = server.EndReceive(ar);
                Console.WriteLine("received : "+received);
            }
            catch
            {
                server.Close();
                return;
            }
            // 받을 데이터가 있다.
            if (received <= 0)
            {
                obj.workingSocket.Close();
                return;
            }

            if (received > 0)
            {
                string text = Encoding.UTF8.GetString(obj.buffer);
                obj.sb.Append(text);
                //Console.WriteLine("sb 내용" + obj.sb.ToString());
                if (server.Available == 0)//네트워크상에 받을 데이터가 없다.
                {
                    Console.WriteLine("수신 종료");
                    // All data received, process it as you wish

                    string receiveData = obj.sb.ToString();
                    string[] tokens = receiveData.Split('\x01');
                    int msgCount = Int32.Parse(tokens[0]); // 비동기 메세지이므로 늦게 온게 먼저 적용된 상태에서 먼저 온게 적용되려는 현상 방지

                    string msg = tokens[1];
                    
                    obj.sb.Clear();//이어가던 내용 초기화(메시지 끝이므로)
                    
                    cmdHandler.CmdClassification(msgCount,msg); // 동기 메서드라 대기하게 된다.
                }
                obj.ClearBuffer();//버퍼 비우기
            }
            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.

            server.BeginReceive(obj.buffer, 0, 2048, 0, DataReceived, obj);


        }
        public void OnSendData(string message, EventArgs e)
        {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound)
            {
                MessageBox.Show("서버가 실행되지 않고 있습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 보낼 텍스트
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("텍스트가 입력되지 않았습니다!", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 서버 ip 주소와 메세지를 담도록 만든다.
            IPEndPoint ip = (IPEndPoint)mainSock.LocalEndPoint;
            string addr = ip.Address.ToString();
            Console.WriteLine("송신 메시지 : "+addr+message);
            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes(addr + '\x01' + message);
            // 서버에 전송한다.
            //mainSock.Send(bDts);
            mainSock.BeginSend(bDts, 0, bDts.Length, 0,
                new AsyncCallback(SendCallback), mainSock);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
