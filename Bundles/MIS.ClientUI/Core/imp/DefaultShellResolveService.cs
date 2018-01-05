using MIS.Foundation.Framework;
using MIS.UI.Framework.Controls;
using OSGi.NET.Core;
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
    public class DefaultShellResolveService : IShellResolveService
    {
        /// <summary>
        /// 其他模块扩展的插件约定数据
        /// </summary>
        private Module mModule;
        private ShellResolveEventHandler ShellResolveEvent;
        private ShellResolveFreamEventHandler ShellResolveFreamEvent;
        private Accordion mCurrentAccordion = null;
        private Bundle mBundle;
        private static Dictionary<String, UserControl> mContainers = new Dictionary<String, UserControl>();
        /// <summary>
        /// 默认插件外壳解析构造函数
        /// </summary>
        /// <param name="node">扩展点数据</param>
        public DefaultShellResolveService(IBundle bundle, ExtensionData extensionData)
        {
            this.mBundle = (Bundle)bundle;
            if (extensionData.Name.Equals("MIS.Shell.Module"))
            {
                if (extensionData.ExtensionList.Count > 1) throw new ExtensionPointNumberException("MIS.Shell.Module扩展点的扩展不允许大于1");
                this.ResolveExtensionDatas(extensionData.ExtensionList[0]);
            }
        }

        /// <summary>
        /// 设置导航回调
        /// </summary>
        /// <param name="handler"></param>
        public void SetShellResolveEventHandler(ShellResolveEventHandler handler)
        {
            this.ShellResolveEvent = handler;
        }

        /// <summary>
        /// 设置菜单回调
        /// </summary>
        /// <param name="handler"></param>
        public void SetShellResolveFreamEventHandler(ShellResolveFreamEventHandler handler)
        {
            this.ShellResolveFreamEvent = handler;
        }
        
        /// <summary>
        /// 根据模块创建导航
        /// </summary>
        public NavigationUIItem CreateNavigationUIItem()
        {
            NavigationUIItem _;
            StringBuilder xamlBuilder = new StringBuilder();
            xamlBuilder.Append("<mis:NavigationUIItem  ");
            xamlBuilder.Append(" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            xamlBuilder.Append(" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            xamlBuilder.Append(" xmlns:mis=\"clr-namespace:MIS.UI.Framework.Controls;assembly=MIS.UI.Framework\"");
            xamlBuilder.Append(String.Format(" Style=\"{{DynamicResource DefaultNavigationUIItemStyle}}\"  Content=\"{0}\" Height=\"88\" Margin=\"0\" HorizontalAlignment=\"Left\" Width=\"87\"/>", this.mModule.Title));
            _ = System.Windows.Markup.XamlReader.Parse(xamlBuilder.ToString()) as NavigationUIItem;
            _.GotFocus += OnGotFocus;
            return _;
        }

        #region 回调事件
        
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (this.ShellResolveEvent != null)
            {
                if (this.mCurrentAccordion == null)
                {
                    this.mCurrentAccordion = this.CreateAccordion();
                }
                this.ShellResolveEvent(new AccordionModel() { ModuleName = this.mModule.Title, Accordion = this.mCurrentAccordion });
            }
        }
        
        private void OnClick(object sender, RoutedEventArgs e)
        {
            ExpanderUIItem newVariable = (sender as ExpanderUIItem);
            if (newVariable != null)
            {
                MenuItem menuItem = newVariable.Tag as MenuItem;
                if (menuItem != null)
                {
                    if (this.ShellResolveFreamEvent != null)
                    {
                        //Create userControl
                        if (!menuItem.Class.Equals(""))
                        {
                            UserControl ctrl = (UserControl)this.mBundle.LoadClass(menuItem.Class);
                            this.ShellResolveFreamEvent(ctrl);
                        }
                        else
                        {
                            this.ShellResolveFreamEvent(new UserControl() { Content = "开发中。。。" });
                        }
                    }
                }
            }

        }
        
        #endregion

        #region 私有方法

        /// <summary>
        /// 创建折叠手风琴对象
        /// </summary>
        /// <returns></returns>
        private Accordion CreateAccordion()
        {
            //声明一个折叠手风琴对象
            Accordion accordion = new Accordion() { Margin = new System.Windows.Thickness(0), HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, VerticalAlignment = System.Windows.VerticalAlignment.Top };
            //声明指定的项            
            foreach (var menu in this.mModule.Menus)
            {
                AccordionItem accordionItem = new AccordionItem();
                //Create Header
                accordionItem.Header = this.CreateAccordionHeader(menu);
                //Create Content
                accordionItem.Content = this.CreateAccordionContent(menu.MenuItems);
                accordion.Items.Add(accordionItem);
            }
            return accordion;
        }

        /// <summary>
        /// 解析XML数据
        /// </summary>
        /// <param name="extensionNode"></param>
        private void ResolveExtensionDatas(XmlNode extensionNode)
        {
            this.mModule = new Module();
            this.mModule.Menus = new List<Menu>();
            foreach (XmlNode module in extensionNode.ChildNodes) //
            {
                if (!(module is XmlComment))
                {
                    //设置模块主要信息
                    this.mModule.Title = module.Attributes["Title"].Value;
                    this.mModule.ToolTip = module.Attributes["ToolTip"].Value;
                    this.mModule.Icon = module.Attributes["Icon"].Value;
                    //读取模块子节点

                    foreach (XmlNode menu in module.ChildNodes)
                    {
                        if (!(menu is XmlComment))
                        {
                            Menu _ = new Menu() { };
                            _.Text = menu.Attributes["Text"].Value;
                            _.ToolTip = menu.Attributes["ToolTip"].Value;
                            _.Icon = menu.Attributes["Icon"].Value;
                            this.mModule.Menus.Add(_);
                            _.MenuItems = new List<MenuItem>();
                            foreach (XmlNode menuItem in menu.ChildNodes)
                            {
                                if (!(menuItem is XmlComment))
                                {
                                    MenuItem __ = new MenuItem() { };
                                    __.Text = menuItem.Attributes["Text"].Value;
                                    __.ToolTip = menuItem.Attributes["ToolTip"].Value;
                                    __.Class = menuItem.Attributes["Class"].Value;
                                    _.MenuItems.Add(__);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 创建折叠手风琴头部信息
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private Grid CreateAccordionHeader(Menu menu)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<Grid ");
            builder.Append(" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
            builder.Append(" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
            builder.Append(" HorizontalAlignment=\"Stretch\" Margin=\"0 0 0 4\">");
            builder.Append("<Grid.ColumnDefinitions>");
            builder.Append("<ColumnDefinition Width=\"*\" />");
            builder.Append("<ColumnDefinition Width=\"*\" />");
            builder.Append("</Grid.ColumnDefinitions>");
            builder.Append(String.Format("<TextBlock Text=\"{0}\" Grid.Column=\"0\" VerticalAlignment=\"Center\" />", menu.Text));
            builder.Append("<Image Grid.Column=\"1\" Height=\"16\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Right\" Source=\"../Resources/leftico01.png\" />");
            builder.Append("</Grid>");
            return System.Windows.Markup.XamlReader.Parse(builder.ToString()) as Grid;
        }

        /// <summary>
        /// 创建折叠手风琴内容信息
        /// </summary>
        /// <param name="menuItems"></param>
        /// <returns></returns>
        private StackPanel CreateAccordionContent(List<MenuItem> menuItems)
        {
            StackPanel _StackPanel = new StackPanel();
            if (menuItems != null)
            {
                foreach (var menuItem in menuItems)
                {
                    StringBuilder xamlBuilder = new StringBuilder();
                    xamlBuilder.Append("<mis:ExpanderUIItem  ");
                    xamlBuilder.Append(" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"");
                    xamlBuilder.Append(" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"");
                    xamlBuilder.Append(" xmlns:mis=\"clr-namespace:MIS.UI.Framework.Controls;assembly=MIS.UI.Framework\"");
                    xamlBuilder.Append(" Height=\"30\" Content=\"" + menuItem.Text + "\" Style=\"{DynamicResource DefaultExpanderUIItem}\"/>");
                    ExpanderUIItem _ = System.Windows.Markup.XamlReader.Parse(xamlBuilder.ToString()) as ExpanderUIItem;
                    _.Click += OnClick;
                    _.Tag = menuItem;
                    _StackPanel.Children.Add(_);
                }
            }
            return _StackPanel;
        }


        #endregion
    }
}
