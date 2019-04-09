using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace socket_server
{
    public partial class Server_form : Form
    {




        struct ClientSocket // 각 client의 소켓과 client가 보고있는 경로를 저장 (폴더삭제, 이동 등의 상황에서 그 경로안을 보고 있는 사용자가 있다면 못하게 막아야하니까?)
        {
            public Socket clientSocket;
            public string clientPath;
        }
        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;
        Socket mainSock;
        IPAddress thisAddress;
        CommandClassification cmdHandler = new CommandClassification();
        int sendMsgcount = 0;
        public Server_form()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            _textAppender = new AppendTextDelegate(AppendText);
        }

        void AppendText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else
            {
                string source = ctrl.Text;
                ctrl.Text = source + Environment.NewLine + s;
            }
        }

        private void Server_form_Load(object sender, EventArgs e)
        {
            IPHostEntry he = Dns.GetHostEntry(Dns.GetHostName());

            thisAddress = IPAddress.Loopback;// 로컬 호스트
        }

        private void Start_server_Click(object sender, EventArgs e)
        {
            Start_server.Enabled = false;
            int port;
            if (!int.TryParse(port_textbox.Text, out port))
            {
                MessageBox.Show("포트 번호가 잘못 입력되었거나 입력되지 않았습니다.", "에러", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                port_textbox.Focus();
                port_textbox.SelectAll();
                return;
            }

            // 서버에서 클라이언트의 연결 요청을 대기하기 위해
            // 소켓을 열어둔다.
            IPEndPoint serverEP = new IPEndPoint(thisAddress, port);
            mainSock.Bind(serverEP);
            mainSock.Listen(10);
            AppendText(server_log_richtextbox, string.Format("서버 동작"));
            // 비동기적으로 클라이언트의 연결 요청을 받는다.
            mainSock.BeginAccept(AcceptCallback, null);
        }

        List<ClientSocket> connectedClients = new List<ClientSocket>();
        void AcceptCallback(IAsyncResult ar)
        {
            // 클라이언트의 연결 요청을 수락한다.
            Socket client = mainSock.EndAccept(ar);

            // 또 다른 클라이언트의 연결을 대기한다.
            mainSock.BeginAccept(AcceptCallback, null);

            AsyncObject obj = new AsyncObject();
            obj.workingSocket = client;

            // 연결된 클라이언트 리스트에 추가해준다.
            ClientSocket clientSocket = new ClientSocket();
            clientSocket.clientSocket = client;
            clientSocket.clientPath = "root";
            connectedClients.Add(clientSocket);

            // 텍스트박스에 클라이언트가 연결되었다고 써준다.
            AppendText(server_log_richtextbox, string.Format("클라이언트 (@ {0})가 연결되었습니다.", client.RemoteEndPoint));

            // 클라이언트의 데이터를 받는다.
            client.BeginReceive(obj.buffer, 0, 2048, 0, DataReceived, obj);
        }

        void DataReceived(IAsyncResult ar)
        {
            // BeginReceive에서 추가적으로 넘어온 데이터를 AsyncObject 형식으로 변환한다.
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            Socket client = obj.workingSocket;
            int received;
            try
            {
                // 데이터 수신을 끝낸다.
                received = client.EndReceive(ar);
                Console.WriteLine("receive : ", received);
            }
            catch
            {
                client.Close();
                return;
            }
            // 받을 데이터가 있다.
            if (received > 0)
            {
                obj.sb.Append(Encoding.UTF8.GetString(obj.buffer, 0, received));
                obj.ClearBuffer();//버퍼 비우기
                if (client.Available == 0)//네트워크상에 받을 데이터가 없다.
                {
                    Console.WriteLine("수신 종료");
                    // All data received, process it as you wish
                    string msg = obj.sb.ToString();
                    obj.sb.Clear();//이어가던 내용 초기화(메시지 끝이므로)
                    Console.WriteLine(msg);//총 받은 메시지
                    string[] tokens = msg.Split('\x01');
                    string ip = tokens[0];
                    string clientData = tokens[1];


                    AppendText(server_log_richtextbox, string.Format("수신 내용 {0} : {1}", ip, clientData));
                    bool sendAll = cmdHandler.IdentifySendAll(clientData);//전체 송신인지 판별
                    //string sendData = cmdHandler.CmdClassification(clientData); //요청사항 결과
                    byte[] sendDatacontent = cmdHandler.CmdClassification(clientData); //요청사항 결과

                    sendMsgcount += 1;
                    byte[] sendDataHeader = Encoding.UTF8.GetBytes(sendMsgcount.ToString() + '\x01');
                    byte[] sendData = new byte[sendDataHeader.Length + sendDatacontent.Length];
                    sendDataHeader.CopyTo(sendData, 0);
                    sendDatacontent.CopyTo(sendData, sendDataHeader.Length);
                    //byte[] sendData = cmdHandler.CmdClassification(clientData); //요청사항 결과
                    Console.WriteLine(sendData.ToString());
                    

                    if (sendAll)//전체전송
                    {
                        // for을 통해 "역순"으로 클라이언트에게 데이터를 보낸다.
                        for (int i = connectedClients.Count - 1; i >= 0; i--)
                        {
                            Socket socket = connectedClients[i].clientSocket;
                            try
                            {
                                socket.Send(sendData);
                            }
                            catch
                            {
                                // 오류 발생하면 전송 취소하고 리스트에서 삭제한다.
                                try { socket.Dispose(); } catch { }
                                connectedClients.RemoveAt(i);
                            }
                        }
                    }
                    else // 1:1전송
                    {
                        AppendText(server_log_richtextbox, string.Format("1:1 전송 내용 : {0}", Encoding.UTF8.GetString(sendData,0,sendData.Length)));
                        client.Send(sendData);
                    }
                    AppendText(server_log_richtextbox, string.Format("연결된 클라이언트 갯수 : {0}", connectedClients.Count));
                }
            }
            
            // 받은 데이터가 없으면(연결끊어짐) 끝낸다.
            if (received <= 0)
            {
                AppendText(server_log_richtextbox, string.Format("연결 종료"));
                obj.workingSocket.Close();
                return;
            }
            

            client.BeginReceive(obj.buffer, 0, 2048, 0, DataReceived, obj);
        }
    }
}
