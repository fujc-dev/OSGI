using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MIS.UI.Framework.Controls
{
    /// <summary>
    /// 图片按钮
    /// </summary>
    public partial class MISImageButton : Button
    {
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
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(MISImageButton), new PropertyMetadata(null));

        public Double IconWidth
        {
            get
            {
                return (Double)GetValue(IconWidthProperty);
            }
            set
            {
                SetValue(IconWidthProperty, value);
            }
        }

        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register("IconWidth", typeof(Double), typeof(MISImageButton), new PropertyMetadata(24D));




        public Double IconHeight
        {
            get
            {
                return (Double)GetValue(IconHeightProperty);
            }
            set
            {
                SetValue(IconHeightProperty, value);
            }
        }

        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register("IconHeight", typeof(Double), typeof(MISImageButton), new PropertyMetadata(24D));

        public CornerRadius MISCornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(MISCornerRadiusProperty);
            }
            set
            {
                SetValue(MISCornerRadiusProperty, value);
            }
        }
        public static readonly DependencyProperty MISCornerRadiusProperty =
            DependencyProperty.Register("MISCornerRadius", typeof(CornerRadius), typeof(MISImageButton), new PropertyMetadata(new CornerRadius(1)));




        public Geometry GeometryIcon
        {
            get
            {
                return (Geometry)GetValue(GeometryIconProperty);
            }
            set
            {
                SetValue(GeometryIconProperty, value);
            }
        }

        public static readonly DependencyProperty GeometryIconProperty =
            DependencyProperty.Register("GeometryIcon", typeof(Geometry), typeof(MISImageButton), new PropertyMetadata(null));
    }
}
