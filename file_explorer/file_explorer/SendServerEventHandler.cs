﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace file_explorer
{
    class SendServerEventHandler
    {
        static ClientSocketHandler clientSocket = new ClientSocketHandler();
        static PathHandler pathHandler=new PathHandler();
        static Stack<string> prePathsave = new Stack<string>();
        static Stack<string> nextPathsave = new Stack<string>();
        static Button backButton;
        static Button nextButton;
        static string mainFormpath="";

        public Stack<string> attributePrepathsave
        {
            get
            {
                return prePathsave;
            }
            set
            {
                prePathsave = value;
            }
        }

        public Stack<string> attributeNextpathsave
        {
            get
            {
                return nextPathsave;
            }
            set
            {
                nextPathsave = value;
            }
        }

        public SendServerEventHandler()
        {
        }

        public void SendServerEventHandlerSetting(Button mainFormbackbutton, Button mainFormnextbutton)
        {
            backButton = mainFormbackbutton;
            nextButton = mainFormnextbutton;
        }
        public string GetMainFormPath()
        {
            return mainFormpath;
        }
        public void MoveDir(string path, bool isClick) // 폴더 이동 부분 
        {
            pathHandler.SetComboxPath(path);
            if (isClick) //클릭형태로 이동이면, 다음 경로 저장해놓은거 날려야됨
            {
                nextPathsave.Clear();
                nextButton.Enabled = false;
                nextButton.BackColor = SystemColors.ScrollBar;
                nextButton.ForeColor = Color.Gray;
            }
            if (path.Equals("root"))
            {
                mainFormpath = "";
                clientSocket.OnSendData("rootload" + "|", null);
                pathHandler.MakePathButton("root");
                if (prePathsave.Count == 0 || !prePathsave.Peek().Equals("root"))
                {
                    prePathsave.Push("root");
                    if (prePathsave.Count > 1)
                    {
                        backButton.Enabled = true;
                        backButton.BackColor = System.Drawing.SystemColors.Window;
                        backButton.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            else
            {
                mainFormpath = path;
                clientSocket.OnSendData("dirload" + "|" + path, null);
                pathHandler.MakePathButton(path);
                if (prePathsave.Count == 0 || !prePathsave.Peek().Equals(path))
                {
                    prePathsave.Push(path);
                    if (prePathsave.Count > 1)
                    {
                        backButton.Enabled = true;
                        backButton.BackColor = System.Drawing.SystemColors.Window;
                        backButton.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }
    }
}