using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace file_explorer
{
    public partial class Main_form : Form
    {
        string userId;
        ClientSocketHandler clientSocket = new ClientSocketHandler();
        LoadDriveInfo loadDriveInfo = new LoadDriveInfo();
        int listViewcount;
        public Main_form(string loginformUserId)
        {
            listViewcount = 0;
            InitializeComponent();
            userId = loginformUserId;
            loadDriveInfo.SetDriveEvnet(SetDriveListView);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            user_name_label.Text = userId;
        }



        private void SetDriveListView(int msgCount, DriveInfo[] datas)
        {
            Console.WriteLine(listViewcount.ToString()+" vs "+msgCount.ToString());
            if (listViewcount < msgCount) // 현재보다 나중의 요청결과인지 확인
            {
                if (mainFormlistview.InvokeRequired)
                {
                    mainFormlistview.Invoke((MethodInvoker)delegate ()
                    {
                        listViewcount = msgCount;
                        mainFormlistview.View = View.Details;
                        mainFormlistview.Columns.Clear();//칼럼 초기화
                        int columWidth = (mainFormlistview.Width - 2) / 4;
                        Console.WriteLine(mainFormlistview.Width);
                        mainFormlistview.Columns.Add("이름");
                        mainFormlistview.Columns.Add("종류");
                        mainFormlistview.Columns.Add("전체 크기");
                        mainFormlistview.Columns.Add("사용 가능 공간");
                        foreach (ColumnHeader header in mainFormlistview.Columns)
                        {
                            header.Width = columWidth;
                        }
                        foreach (DriveInfo data in datas)
                        {
                            Console.WriteLine(data);
                            ListViewItem item = new ListViewItem(new[] { data.driveName, data.driveType, data.driveTotalsize, data.driveFreesize });
                            mainFormlistview.Items.Add(item);
                        }
                    });
                }
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clientSocket.OnSendData("rootload" + ";", null);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
