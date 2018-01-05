using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http
{
    public class Message
    {
        public Message()
        {
            this.State = ResultState.Success;
            this.Msg = "成功";
        }
        public ResultState State { get; set; }

        public string Msg { get; set; }
    }
}
