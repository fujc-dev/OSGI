using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace MIS.ClientUI
{
    public partial class MISLogin : Window
    {
        public MISLogin()
        {
            InitializeComponent();
            
            DataContext = this;
            _BindCommand();
            _CenterWindowOnScreen();
        }

        private void _CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }

        public ICommand ExitCommand { get; private set; }
        public ICommand ForgetPasswordCommand { get; private set; }
        public ICommand HelpCommand { get; private set; }
        public ICommand LoginCommand { get; private set; }


        private void BindCommand(UIElement @ui, ICommand com, Action<object, ExecutedRoutedEventArgs> call)
        {
            var bind = new CommandBinding(com);
            bind.Executed += new ExecutedRoutedEventHandler(call);
            @ui.CommandBindings.Add(bind);
        }

        private void _BindCommand()
        {
            ExitCommand = new RoutedUICommand();
            ForgetPasswordCommand = new RoutedUICommand();
            HelpCommand = new RoutedUICommand();
            LoginCommand = new RoutedUICommand();
            BindCommand(this, LoginCommand, (sender, eventArgs) =>
            {
                new MainWindow().Show();
                Close();
            });
            BindCommand(this, ForgetPasswordCommand, (sender, eventArgs) =>
            {
            });
            BindCommand(this, HelpCommand, (sender, eventArgs) =>
            {
            });
            BindCommand(this, ExitCommand, (s, e) =>
            {
                Application.Current.Shutdown();
            });
        }
    }
}
