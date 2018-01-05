using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIS.ClientUI.Core
{

    public class MenuItem
    {
        [XmlAttribute("Text")]
        public String Text { get; set; }

        [XmlAttribute("ToolTip")]
        public String ToolTip { get; set; }

        [XmlAttribute("Class")]
        public String Class { get; set; }
    }
}
