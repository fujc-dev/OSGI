
	Bundles模块概要：


			var stateStr = string.Empty;
            if (e.GetState() == ExtensionEventArgs.LOAD)
            {
                stateStr = "Load";
            }
            else
            {
                if (e.GetState() == ExtensionEventArgs.UNLOAD)
                {
                    stateStr = "UnLoad";
                }
            }
            var extensionStr = new StringBuilder();
            foreach (var xmlNode in e.GetExtensionData().ExtensionList)
            {
                extensionStr.Append(xmlNode.InnerXml);
            }
            MessageBox.Show(string.Format("{0} {1} {2} Extension {3}", ((IBundle)sender).GetSymbolicName(), stateStr, e.GetExtensionPoint().Name, extensionStr));