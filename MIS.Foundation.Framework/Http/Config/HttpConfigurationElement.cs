using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http.Config
{
    public class HttpConfigurationSectionElement : ConfigurationElement
    {
        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return this["host"].ToString(); }
            set { this["host"] = value; }
        }
    }
}
