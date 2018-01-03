using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZHGL.Utils
{
    /// <summary>
    /// Log4NetLogFactory
    /// </summary>
    public class Log4NetLogFactory : LogFactoryBase
    {
        public Log4NetLogFactory()
            : this("log4net.config")
        {

        }

        public Log4NetLogFactory(string log4netConfig)
            : base(log4netConfig)
        {
            if (!IsSharedConfig)
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(ConfigFile));
            }
            else
            {
                //Disable Performance logger
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigFile);
                var docElement = xmlDoc.DocumentElement;
                var perfLogNode = docElement.SelectSingleNode("logger[@name='Performance']");
                if (perfLogNode != null)
                    docElement.RemoveChild(perfLogNode);
                log4net.Config.XmlConfigurator.Configure(docElement);
            }
        }
        public override ILog GetLog(string name)
        {
            return new Log4NetLog(LogManager.GetLogger(name));
        }
    }
}
