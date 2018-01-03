using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHGL.Utils.Logging.Config
{
    public class LogConfigurationSection : ConfigurationSection
    {

        [ConfigurationProperty("zhgllog", IsRequired = true)]
        public LogConfigurationSectionElement ZhglLog
        {
            get { return (LogConfigurationSectionElement)this["zhgllog"]; }
        }
    }
}
