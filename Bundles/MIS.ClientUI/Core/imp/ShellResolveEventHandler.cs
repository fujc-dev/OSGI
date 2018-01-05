using MIS.UI.Framework.Controls;
using OSGi.NET.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace MIS.ClientUI.Core
{
    public delegate void ShellResolveEventHandler(AccordionModel ccordion);

    public delegate void ShellResolveFreamEventHandler(UserControl control);

    public class AccordionModel
    {
        public Accordion Accordion { get; set; }
        public String ModuleName { get; set; }

    }
}
