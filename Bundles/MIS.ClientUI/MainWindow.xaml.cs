using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

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
            WindowState = System.Windows.WindowState.Normal;
            WindowStyle = System.Windows.WindowStyle.None;
            ResizeMode = System.Windows.ResizeMode.NoResize;
            Left = 0.0;
            Top = 0.0;
            Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
    }
}
