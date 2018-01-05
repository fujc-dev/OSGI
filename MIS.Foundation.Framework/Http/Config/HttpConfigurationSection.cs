using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIS.Foundation.Framework.Http.Config
{
    public class HttpConfigurationSection : ConfigurationSection
    {

        [ConfigurationProperty("zhglhttp", IsRequired = true)]
        public HttpConfigurationSectionElement ZhglHttp
        {
            get { return (HttpConfigurationSectionElement)this["zhglhttp"]; }
        }

        [ConfigurationProperty("zthttp", IsRequired = true)]
        public HttpConfigurationSectionElement ZTHttp
        {
            get { return (HttpConfigurationSectionElement)this["zthttp"]; }
        }
    }
}
