using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MIS.Foundation.Framework
{
    public class LogConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("log4net", IsRequired = true)]
        public LogConfigurationSectionElement Log4net
        {
            get
            {
                return (LogConfigurationSectionElement)this["log4net"];
            }
        }
    }
}
