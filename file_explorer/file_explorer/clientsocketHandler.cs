using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace file_explorer
{
    public delegate void socket_eventHandler(string data);
    
    class clientsocketHandler
    {
        static event socket_eventHandler socket_event;
        static Socket mainSock;
        static clientsocketHandler()
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
            AsyncObject ao = new AsyncObject(4096);
            ao.WorkingSocket = mainSock;
            mainSock.BeginReceive(ao.Buffer, 0, ao.BufferSize, 0, DataReceived, ao);
        }

        static void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.
            AsyncObject obj = (AsyncObject)ar.AsyncState;

            // 데이터 수신을 끝낸다.
            int received = obj.WorkingSocket.EndReceive(ar);

            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
            if (received <= 0)
            {
                obj.WorkingSocket.Close();
                return;
            }

            // UTF8 인코더를 사용하여 바이트 배열을 문자열로 변환한다.
            string text = Encoding.UTF8.GetString(obj.Buffer);

            // 0x01 기준으로 짜른다.
            // tokens[0] - 보낸 사람 IP
            // tokens[1] - 보낸 메세지
            string[] tokens = text.Split('\x01');
            string ip = tokens[0];
            string msg = tokens[1];
            // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
            // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
            string[] msgs = msg.Split(';');
            
            if (msgs[0].Equals("success"))
            {
                socket_event("success");
            }
            obj.ClearBuffer();

            // 수신 대기
            obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0, DataReceived, obj);
        }
        public void set_socket_evnet(socket_eventHandler add_event)
        {
            socket_event+= new socket_eventHandler(add_event);
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

            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes(addr + '\x01' + message);

            // 서버에 전송한다.
            mainSock.Send(bDts);
        }
    }
}
