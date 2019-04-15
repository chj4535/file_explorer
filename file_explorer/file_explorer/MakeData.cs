using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    public delegate void ChangeStateEventHandler(object[] data);
    //public delegate void ChangeStateEventHandler(string type,string state,object);
    class MakeData
    {
        LoginCheck loginCheck = new LoginCheck();
        static List<object[]> dataSet = new List<object[]>();
        static List<string> dataSetIndex = new List<string>();
        static event ChangeStateEventHandler changeStateevent;
        public void SetDriveInfoToCurrentStateEventHandler(ChangeStateEventHandler addEvent)
        {
            changeStateevent += new ChangeStateEventHandler(addEvent);
        }

        public void MakeDataSet(string msg)
        {

            string[] msgs = msg.Split('|');
            string msgCount = msgs[0];
            string itemCount = msgs[1];
            string target = msgs[2];
            if (msgs[3].Equals("login"))
            {
                loginCheck.LoginResult(msgs[4]);
            }
            else
            {
                if (!dataSetIndex.Contains(msgCount)) //최초로 받은 메시지
                {
                    dataSetIndex.Add(msgCount);
                    dataSet.Add(new object[Int32.Parse(itemCount) + 3]); //0번방에 현재 갯수, 1번방에 경로저장되므로 2칸 더 많아야됨
                    dataSet[dataSet.Count - 1][0] = 0; //현재 개수
                    dataSet[dataSet.Count - 1][1] = Int32.Parse(itemCount);// 총 개수
                    dataSet[dataSet.Count - 1][2] = target;// 타겟
                }
                int msgIndex = dataSetIndex.IndexOf(msgCount); //메시지 위치 찾음
                int dataCount = (int)(dataSet[msgIndex][0]);
                dataCount++;
                //msgs[2] : type
                //msgs[3] : state
                //msgs[4] : info
                dataSet[msgIndex][dataCount + 2] = msgs[3] + '|' + msgs[4] + '|' + msgs[5] + '|';
                dataSet[msgIndex][0] = dataCount;
                if ((int)dataSet[msgIndex][0] == (int)dataSet[msgIndex][1])//메시지 다 바음
                {
                    changeStateevent(dataSet[msgIndex]);
                    dataSet.RemoveAt(msgIndex);
                    dataSetIndex.RemoveAt(msgIndex);
                }
            }
        }
    }
}