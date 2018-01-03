using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MIS.Foundation.Framework
{
    public class LogConfigurationSectionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"].ToString();
            }
            set
            {
                this["name"] = value;
            }
        }
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get
            {
                return this["type"].ToString();
            }
            set
            {
                this["type"] = value;
            }
        }
    }
}
