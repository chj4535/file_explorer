using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace socket_server
{
    class Logincheck
    {
        string[][] userChart = new string[][] { new string[] { "admin", "1234" }, new string[] { "admin2", "1q2w3e4r" }, new string[] { "choi", "112233" } };
        public byte[] GetloginInfo(string msg)
        {
            string[] msgs = msg.Split('/');
            if (CompareInfo(msgs[0], msgs[1]))
            {
                return Encoding.UTF8.GetBytes("success"+'|');
            }
            return Encoding.UTF8.GetBytes("fail"+'|');
        }

        private bool CompareInfo(string id,string pw)
        {
            foreach(string[] user in userChart)
            {
                if(id.Equals(user[0]) && pw.Equals(user[1]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
