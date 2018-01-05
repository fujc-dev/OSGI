using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MIS.UI.Framework.Controls
{
    /// <summary>
    /// LinkButton(链接按钮)
    /// </summary>
    public partial class MISLinkButton : Button
    {
        public Brush LBForeground
        {
            get
            {
                return (Brush)GetValue(LBForegroundProperty);
            }
            set
            {
                SetValue(LBForegroundProperty, value);
            }
        }

        public static readonly DependencyProperty LBForegroundProperty =
            DependencyProperty.Register("LBForeground", typeof(Brush), typeof(MISLinkButton), new PropertyMetadata(null));


        public Brush LBMouseOverForeground
        {
            get
            {
                return (Brush)GetValue(LBMouseOverForegroundProperty);
            }
            set
            {
                SetValue(LBMouseOverForegroundProperty, value);
            }
        }

        public static readonly DependencyProperty LBMouseOverForegroundProperty =
            DependencyProperty.Register("LBMouseOverForeground", typeof(Brush), typeof(MISLinkButton), new PropertyMetadata(null));

        public Brush LBPressdForeground
        {
            get
            {
                return (Brush)GetValue(LBPressdForegroundProperty);
            }
            set
            {
                SetValue(LBPressdForegroundProperty, value);
            }
        }

        public static readonly DependencyProperty LBPressdForegroundProperty =
            DependencyProperty.Register("LBPressdForeground", typeof(Brush), typeof(MISLinkButton), new PropertyMetadata(null));


        public ImageSource Source
        {
            get
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value);
            }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(MISLinkButton), new PropertyMetadata(null));




        public String ValueEx
        {
            get { return (String)GetValue(ValueExProperty); }
            set { SetValue(ValueExProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ValueEx.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueExProperty =
            DependencyProperty.Register("ValueEx", typeof(String), typeof(MISLinkButton), new PropertyMetadata(""));


    }
}
