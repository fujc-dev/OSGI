using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MIS.ClientUI.Behaviors
{
    public class MouseDragFrameworkElementBehavior : Behavior<FrameworkElement>
    {
        public MouseDragFrameworkElementBehavior()
        {
        }
        private Window m_CurrentWindow;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewMouseLeftButtonDown += OnDragMove;
        }

        private void OnDragMove(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(AssociatedObject);
            if (position.X < AssociatedObject.ActualWidth && position.Y < AssociatedObject.ActualHeight)
            {
                if (m_CurrentWindow == null)
                {
                    m_CurrentWindow = Window.GetWindow(AssociatedObject);
                }
                m_CurrentWindow.DragMove();
            }
        }
    }
}
