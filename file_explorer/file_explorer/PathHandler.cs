using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace file_explorer
{
    class PathHandler
    {
        static ComboBox mainPathcombobox = new ComboBox();
        static ComboBox mainRecentcombobox = new ComboBox();
        static Button mainpathButton = new Button();
        static Graphics graphic;
        static List<string> mainPathitem = new List<string>();
        static SendServerEventHandler sendServerEventHandler=new SendServerEventHandler();
        public PathHandler()
        {
        }
        
        public void SetErrorPath(string path)
        {
            SetErrorRecentComboBox(path);
            SetErrorPathComboBox(path);
        }

        private void SetErrorRecentComboBox(string path)
        {
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    SetErrorRecentComboBox(path);
                });
                return;
            }
            string recentPath;
            if (path.Split('\\').Last().Equals(""))
            {
                recentPath = path.Split('\\').First();
            }
            else
            {
                recentPath = path.Split('\\').Last();
            }
            int resultindex = mainRecentcombobox.FindStringExact(recentPath);
            if (resultindex != -1)
            {
                mainRecentcombobox.Items.RemoveAt(resultindex);
            }
        }

        private void SetErrorPathComboBox(string path)
        {
            if (mainPathcombobox.InvokeRequired)
            {
                mainPathcombobox.Invoke((MethodInvoker)delegate {
                    SetPathComboBox(path);
                });
                return;
            }
            int resultindex = mainPathcombobox.FindStringExact(path);
            if (resultindex != -1)
            {
                mainPathcombobox.Items.RemoveAt(resultindex);
            }
            mainPathitem.Remove(path);
        }

        public void PathHandlerSetting(Button mainFormpathbutton, ComboBox mainFormcombobox, ComboBox mainFormrecentcombobox, Graphics mainFormgraphic)
        {
            mainpathButton = mainFormpathbutton;
            mainPathcombobox = mainFormcombobox;
            mainRecentcombobox = mainFormrecentcombobox;
            graphic = mainFormgraphic;
        }

        public void SetComboxPath(string path)
        {
            SetRecentComboBox(path);
            SetPathComboBox(path);
        }
        private void SetRecentComboBox(string path)
        {
            if (mainRecentcombobox.InvokeRequired)
            {
                mainRecentcombobox.Invoke((MethodInvoker)delegate {
                    SetRecentComboBox(path);
                });
                return;
            }
            if (path.Split('\\').Last().Equals(""))
            {
                SizeF size = graphic.MeasureString(path.Split('\\').First(), mainRecentcombobox.Font);
                if(mainRecentcombobox.DropDownWidth < size.Width)
                {
                    mainRecentcombobox.DropDownWidth = (int)size.Width+2;
                }
                mainRecentcombobox.Items.Add(new { Text = path.Split('\\').First(), Value = path });
            }
            else
            {
                SizeF size = graphic.MeasureString(path.Split('\\').Last(), mainRecentcombobox.Font);
                if (mainRecentcombobox.DropDownWidth < size.Width)
                {
                    mainRecentcombobox.DropDownWidth = (int)size.Width + 2;
                }
                mainRecentcombobox.Items.Add(new { Text = path.Split('\\').Last(), Value = path });
            }
            if (mainRecentcombobox.Items.Count > 10)
            {
                mainRecentcombobox.Items.RemoveAt(0);
            }
        }
        private void SetPathComboBox(string path)
        {
            if (mainPathcombobox.InvokeRequired)
            {
                mainPathcombobox.Invoke((MethodInvoker)delegate {
                    SetPathComboBox(path);
                });
                return;
            }
            mainPathcombobox.Items.Clear();
            mainPathcombobox.Text = path;
            if (!mainPathitem.Contains(path))
            {
                mainPathitem.Add(path);
            }
            foreach(string item in mainPathitem)
            {
                mainPathcombobox.Items.Add(item);
            }
            /*
            if (!mainPathcombobox.Items.Contains(path));
            {
                mainPathcombobox.Items.Add(path);
            }
            */
        }

        public void MakePathButton(string dirPath) //path경로에 따른 버튼 추가
        {
            if (mainpathButton.InvokeRequired)
            {
                mainpathButton.Invoke((MethodInvoker)delegate {
                    MakePathButton(dirPath);
                });
                return;
            }
            //Graphics graphic = this.CreateGraphics();
            mainpathButton.Controls.Clear();
            //mainFormcombobox.Text = "내 PC";
            Button mainButton = new Button();
            mainButton.Location = new Point(0, 0);
            mainButton.Cursor = Cursors.Default;
            mainButton.Text = "내 PC";
            mainButton.Name = "root";
            SizeF size = graphic.MeasureString("내 PC", mainButton.Font);
            mainButton.Width = (int)size.Width + 20;
            mainButton.Height = mainpathButton.Height;


            mainButton.Click += (s, e) => { sendServerEventHandler.MoveDir("root", true,"rootpathbuttonclick"); };
            //mainButton.BringToFront();
            mainpathButton.Controls.Add(mainButton);

            if (!dirPath.Equals("root"))
            {
                string[] dirPaths = dirPath.Split('\\');
                int buttonCount = 0;
                int preButtonpoint = 0;
                string path = "";
                foreach (string dirName in dirPaths)
                {
                    if (dirName.Equals("")) break;
                    preButtonpoint += mainpathButton.Controls[buttonCount].Size.Width - 2;
                    Button dirButton = new Button();
                    dirButton.Location = new Point(preButtonpoint, 0);
                    dirButton.Cursor = Cursors.Default;
                    //dirButton.Size = new Size(25, mainFormcombobox.Height-3);
                    //dirButton.AutoSize = true;
                    //dirButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    //dirButton.BringToFront();
                    dirButton.Text = dirName;
                    size = graphic.MeasureString(dirName, mainButton.Font);
                    dirButton.Width = (int)size.Width + 15;
                    dirButton.Height = mainpathButton.Height;


                    path += dirName;
                    if (path.Split('\\').Length < 2)
                    {
                        dirButton.Name = path+'\\';
                    }
                    else
                    {
                        dirButton.Name = path;
                    }
                    dirButton.Click += (s, e) => { sendServerEventHandler.MoveDir(dirButton.Name, true,"dirpathbuttonclick"); };
                    mainpathButton.Controls.Add(dirButton);
                    buttonCount += 1;
                    path += "\\";
                }
            }

        }
    }
}
