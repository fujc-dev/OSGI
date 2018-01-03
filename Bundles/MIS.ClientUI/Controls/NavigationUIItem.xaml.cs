using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MIS.ClientUI.Controls
{
    /// <summary>
    /// 自定义管理系统顶部导航选项卡按钮
    /// </summary>
    public partial class NavigationUIItem : Button
    {
        public ImageSource MousePressdBackGroundImage
        {
            get
            {
                return (ImageSource)GetValue(MousePressdBackGroundImageProperty);
            }
            set
            {
                SetValue(MousePressdBackGroundImageProperty, value);
            }
        }
        public static readonly DependencyProperty MousePressdBackGroundImageProperty =
            DependencyProperty.Register("MousePressdBackGroundImage", typeof(ImageSource), typeof(NavigationUIItem), new PropertyMetadata(new BitmapImage(new Uri("../images/navbg.png", UriKind.Relative))));

        public ImageSource NavigationModuleIcon
        {
            get
            {
                return (ImageSource)GetValue(NavigationModuleIconProperty);
            }
            set
            {
                SetValue(NavigationModuleIconProperty, value);
            }
        }
        public static readonly DependencyProperty NavigationModuleIconProperty =
            DependencyProperty.Register("NavigationModuleIcon", typeof(ImageSource), typeof(NavigationUIItem), new PropertyMetadata(null));
    }
}
