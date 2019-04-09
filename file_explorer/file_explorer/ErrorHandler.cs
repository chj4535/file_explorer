using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace file_explorer
{
    //public delegate void ReceiveToServerErrorHandler(int msgCount, string errorMessage);
    public delegate void SendtoMainFormErrorHandler(int msgCount, string errorMessage);
    class ErrorHandler
    {
        //static event ReceiveToServerErrorHandler receiveToservererrorhandler;
        static event SendtoMainFormErrorHandler sendToformerrorhandler;

        public void ReceiveToServerError(int msgCount,string errorMessage)
        {
            sendToformerrorhandler(msgCount,errorMessage);//Mainform에 에러 보내기
        }

        public void SetSendtoMainFormErrorEvent(SendtoMainFormErrorHandler addEvent)
        {
            sendToformerrorhandler += new SendtoMainFormErrorHandler(addEvent);
        }
    }
}
