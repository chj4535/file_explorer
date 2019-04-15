using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;

namespace file_explorer
{
    class ButtonHandler : CurrentState
    {
        static Button backButton;
        static Button nextButton;
        static Button upperButton;
        static DateTime preRefreshtime=DateTime.Now;
        public void ButtonHandlerSetting(Button mainFormbackbutton, Button mainFormnextbutton, Button mainFormupperbutton)
        {
            backButton = mainFormbackbutton;
            nextButton = mainFormnextbutton;
            upperButton = mainFormupperbutton;
            backButton.Enabled = false;
            nextButton.Enabled = false;
            upperButton.Enabled = false;
            backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            backButton.ForeColor = System.Drawing.Color.Gray;
            nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
            nextButton.ForeColor = System.Drawing.Color.Gray;
        }
        public void Update()
        {
            if (nextButton.InvokeRequired)
            {
                nextButton.Invoke((MethodInvoker)delegate {
                    Update();
                });
                return;
            }
            if (upperButton.InvokeRequired)
            {
                upperButton.Invoke((MethodInvoker)delegate {
                    Update();
                });
                return;
            }
            if (backButton.InvokeRequired)
            {
                backButton.Invoke((MethodInvoker)delegate {
                    Update();
                });
                return;
            }
            if (isClick)
            {
                nextPathsave.Clear();
                nextButton.Enabled = false;
                nextButton.BackColor = SystemColors.ScrollBar;
                nextButton.ForeColor = Color.Gray;
            }

            if (currentStaticpath.Equals("root"))
            {
                upperButton.Enabled = false;
                upperButton.BackColor = SystemColors.ScrollBar;
                upperButton.ForeColor = Color.Gray;
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
                upperButton.Enabled = true;
                upperButton.BackColor = System.Drawing.SystemColors.Window;
                upperButton.ForeColor = Color.Black;
                if (prePathsave.Count == 0 || !prePathsave.Peek().Equals(currentStaticpath))
                {
                    prePathsave.Push(currentStaticpath);
                    if (prePathsave.Count > 1)
                    {
                        backButton.Enabled = true;
                        backButton.BackColor = System.Drawing.SystemColors.Window;
                        backButton.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
        }
        public void backButtonclick()
        {
            nextPathsave.Push(prePathsave.Pop()); //현재 경로를 다음스택에 저장해놓음
            if (prePathsave.Count == 1)
            {
                backButton.Enabled = false;
                backButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                backButton.ForeColor = System.Drawing.Color.Gray;
            }
            nextButton.Enabled = true;
            nextButton.BackColor = System.Drawing.SystemColors.Window;
            nextButton.ForeColor = System.Drawing.Color.Black;
            isClick = false;
            currentStaticpath = prePathsave.Peek();
            sendServerEventHandler.MoveDir(prePathsave.Peek(), "backbutton");
        }

        public void nextButtonclick()
        {
            prePathsave.Push(nextPathsave.Pop()); //현재 경로를 다음스택에 저장해놓음
            if (nextPathsave.Count == 0)
            {
                nextButton.Enabled = false;
                nextButton.BackColor = System.Drawing.SystemColors.ScrollBar;
                nextButton.ForeColor = System.Drawing.Color.Gray;
            }
            backButton.Enabled = true;
            backButton.BackColor = System.Drawing.SystemColors.Window;
            backButton.ForeColor = System.Drawing.Color.Black;
            isClick = false;
            currentStaticpath = prePathsave.Peek();
            sendServerEventHandler.MoveDir(prePathsave.Peek(), "nextbutton");
        }

        public void upperButtonclick()
        {
            string[] dirPaths = currentStaticpath.Split('\\');
            string currentPath;
            if (dirPaths.Length == 2 && dirPaths[1].Equals(""))//드라이브
            {
                currentPath = "root";
            }
            else//폴더
            {
                currentPath = currentStaticpath;
                currentPath = currentPath.Substring(0, currentPath.LastIndexOf('\\'));
                currentPath = currentPath.Substring(0, currentPath.LastIndexOf('\\'));
                currentPath = currentPath + '\\';
            }
            isClick = true;
            currentStaticpath = currentPath;
            sendServerEventHandler.MoveDir(currentPath, "upperbutton");
        }

        public void resetbutton(DateTime refreshTime)
        {
            TimeSpan time = new TimeSpan(0, 0, 5);
            if (refreshTime > preRefreshtime.Add(time))
            {
                preRefreshtime = refreshTime;
                sendServerEventHandler.reload(currentStaticpath);
            }
        }
    }
}
