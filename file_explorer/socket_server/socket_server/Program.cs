using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace socket_server
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Server_form());
        }
    }
}
