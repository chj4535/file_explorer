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
        static SendServerEventHandler sendServerEventHandler=new SendServerEventHandler();
        public PathHandler()
        {
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
            if (path.Split('\\').Last().Equals(""))
            {
                mainRecentcombobox.Items.Add(new { Text = path.Split('\\').First(),Value = path });
            }
            else
            {
                mainRecentcombobox.Items.Add(new { Text = path.Split('\\').Last(), Value = path });
            }
            if (mainRecentcombobox.Items.Count > 10)
            {
                mainRecentcombobox.Items.RemoveAt(0);
            }
            mainPathcombobox.Text = path;
            if (!mainPathcombobox.Items.Contains(path))
            {
                mainPathcombobox.Items.Add(path);
            }
        }

        public void MakePathButton(string dirPath) //path경로에 따른 버튼 추가
        {
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


            mainButton.Click += (s, e) => { sendServerEventHandler.MoveDir("root", true); };
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


                    path += dirName + "\\";
                    dirButton.Name = path;
                    dirButton.Click += (s, e) => { sendServerEventHandler.MoveDir(dirButton.Name, true); };
                    mainpathButton.Controls.Add(dirButton);
                    buttonCount += 1;
                }
            }

        }
    }
}
