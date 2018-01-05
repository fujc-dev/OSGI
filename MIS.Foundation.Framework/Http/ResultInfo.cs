using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http
{
    public class ResultInfo<T>
    {
        public ResultInfo()
        {
            this.Message = new Message();
        }
        public T Data { get; set; }

        public Message Message { get; set; }
    }
}
