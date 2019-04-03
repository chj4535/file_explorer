using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace socket_server
{
    class CommandClassification
    {
        Logincheck loginCheck = new Logincheck();
        public byte[] CmdClassification(string msg)
        {
            string[] msgs = msg.Split(';');
            string[] returnMessages = new string[2];
            Console.WriteLine("msgs[0] : " + msgs[0]);
            Console.WriteLine("msgs[1] : " + msgs[1]);
            Console.WriteLine("msgs[2] : " + msgs[2]);
            switch (msgs[0]) {
                case "login":
                    returnMessages[0] = "login";
                    returnMessages[1] = loginCheck.GetloginInfo(msgs[1], msgs[2]);
                    break;
            }
            string mesasage = "";
            Console.WriteLine("returnMessages[0] : " + returnMessages[0]);
            Console.WriteLine("returnMessages[1] : " + returnMessages[1]);
            foreach (string returnMessage in returnMessages)
            {
                Console.WriteLine("returnMessage : " + returnMessage);
                mesasage += returnMessage;
                mesasage += ";";
            }
            Console.WriteLine("mesasage : " + mesasage);
            byte[] data = Encoding.UTF8.GetBytes(mesasage);
            return data;
        }

        public bool IdentifySendAll(string msg)
        {
            string[] msgs = msg.Split(';');
            bool sendAll = false;
            switch (msgs[0])
            {
                case "login":
                    sendAll = false;
                    break;
            }
            return sendAll;
        }
    }
}
