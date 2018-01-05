using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MIS.ClientUI.Core;

namespace MIS.ClientUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //设置窗体信息
            SetWindowLocation();
            //加载模块
            foreach (var resolveService in Bootstrap.GetService())
            {
                this.Navigation.Children.Add(resolveService.CreateNavigationUIItem());
                resolveService.SetShellResolveEventHandler((o) =>
                {
                    this.Accordion.Child = o.Accordion;
                });
                resolveService.SetShellResolveFreamEventHandler((o) =>
                {
                    o.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    o.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    this.Fream.Child = o;
                });
            }
            //设置默认选中项
            this.Navigation.Children[0].Focus();
            this.Fream.Child = new MIS.ClientUI.Views.MISIndex() { VerticalAlignment = System.Windows.VerticalAlignment.Top };
            //IShellResolveService resolveService = Bootstrap.GetService()[0];
            //this.Navigation.Children.Add(resolveService.CreateNavigationUIItem());
            //this.Accordion.Child = resolveService.CreateAccordion();
        }

        #region 私有方法
        private void SetWindowLocation()
        {
            WindowState = System.Windows.WindowState.Normal;
            WindowStyle = System.Windows.WindowStyle.None;
            ResizeMode = System.Windows.ResizeMode.NoResize;
            Left = 0.0;
            Top = 0.0;
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
        #endregion
    }
}
