using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MIS.UI.Framework.Controls
{
    /// <summary>
    /// 盒子控件
    /// </summary>
    [TemplatePart(Name = MISBoxItem.MIS_PART_Title, Type = typeof(TextBlock))]
    [TemplatePart(Name = MISBoxItem.MIS_PART_Opration, Type = typeof(MISLinkButton))]
    public partial class MISBoxItem : ContentControl
    {
        private const String MIS_PART_Title = "PART_Title";  //标题显示
        private const String MIS_PART_Opration = "PART_Opration"; //操作

        private TextBlock PART_Title;
        private MISLinkButton PART_Opration;

        public override void OnApplyTemplate()
        {
            this.PART_Title = GetTemplateChild(MISBoxItem.MIS_PART_Title) as TextBlock;
            this.PART_Opration = GetTemplateChild(MISBoxItem.MIS_PART_Opration) as MISLinkButton;
            base.OnApplyTemplate();
        }



        #region 控件标题
        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(MISBoxItem), new FrameworkPropertyMetadata("常用工具",
            OnTitlePropertyChanged));

        //属性改变
        static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MISBoxItem ctrl = (MISBoxItem)d;
            if (ctrl.PART_Title != null)
            {
                ctrl.PART_Title.Text = e.NewValue.ToString();
            }
        }
        #endregion

        #region 操作更多
        public String OpreationValue
        {
            get { return (String)GetValue(OpreationValueProperty); }
            set { SetValue(OpreationValueProperty, value); }
        }
        public static readonly DependencyProperty OpreationValueProperty =
            DependencyProperty.Register("OpreationValue", typeof(String), typeof(MISBoxItem), new FrameworkPropertyMetadata("添加",
                OnOpreationValuePropertyChanged)
            );

        static void OnOpreationValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MISBoxItem ctrl = (MISBoxItem)d;
            if (ctrl.PART_Opration != null)
            {
                ctrl.PART_Opration.Content = e.NewValue.ToString();
            }            
        }
        #endregion



        






    }
}
