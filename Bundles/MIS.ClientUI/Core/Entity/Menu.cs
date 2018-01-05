using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIS.ClientUI.Core
{
    public class Menu
    {
        [XmlAttribute("Icon")]
        public String Icon { get; set; }

        [XmlAttribute("Text")]
        public String Text { get; set; }

        [XmlAttribute("ToolTip")]
        public String ToolTip { get; set; }

        [XmlElement("MenuItem")]
        public List<MenuItem> MenuItems { get; set; }
    }
}
