using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MIS.ClientUI.Core
{
    [XmlRoot("Module")]
    public class Module : XBase
    {
        [XmlAttribute("Icon")]
        public String Icon { get; set; }

        [XmlElement("Menu")]
        public List<Menu> Menus { get; set; }
    }
}
