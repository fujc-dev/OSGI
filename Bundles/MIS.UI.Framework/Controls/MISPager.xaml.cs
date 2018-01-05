using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MIS.UI.Framework.Controls
{
    /// <summary>
    /// 分页控件
    /// </summary>
    [TemplatePart(Name = MISPager.MIS_PART_CONTENT, Type = typeof(StackPanel))]
    [TemplatePart(Name = MISPager.MIS_PART_PREVIOUSPAGE, Type = typeof(MISImageButton))]
    [TemplatePart(Name = MISPager.MIS_PART_NEXTPAGE, Type = typeof(MISImageButton))]
    [TemplatePart(Name = MISPager.MIS_PART_COUNT, Type = typeof(TextBlock))]
    [TemplatePart(Name = MISPager.MIS_PART_PAGEINDEX, Type = typeof(TextBlock))]
    public partial class MISPager : Control
    {
        private const String MIS_PART_CONTENT = "PART_Content";
        private const String MIS_PART_PREVIOUSPAGE = "PART_Previouspage";
        private const String MIS_PART_NEXTPAGE = "PART_Nextpage";
        private const String MIS_PART_COUNT = "PART_Count";
        private const String MIS_PART_PAGEINDEX = "PART_PageIndex";

        private MISImageButton PART_Nextpage;
        private MISImageButton PART_Previouspage;
        private StackPanel PART_Content;
        private TextBlock PART_Count;
        private TextBlock PART_PageIndex;

        private PagerType mPagerType = PagerType.Default;
        private List<Int32> mCurrentPagers = new List<Int32>();
        private Boolean mCurrentIsAddEllipsisCtrl = false;
        public MISPager()
        {
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Content = GetTemplateChild(MISPager.MIS_PART_CONTENT) as StackPanel;
            PART_Nextpage = GetTemplateChild(MISPager.MIS_PART_NEXTPAGE) as MISImageButton;
            PART_Previouspage = GetTemplateChild(MISPager.MIS_PART_PREVIOUSPAGE) as MISImageButton;
            PART_Count = GetTemplateChild(MISPager.MIS_PART_COUNT) as TextBlock;
            PART_PageIndex = GetTemplateChild(MISPager.MIS_PART_PAGEINDEX) as TextBlock;
            PageCount = (Int32)Math.Ceiling((Double)Total / (Double)PageSize);
            PART_Count.Text = Total.ToString();
            if (PageCount <= 7)
            {
                mPagerType = PagerType.Default;
                for (var i = 0; i < PageCount; i++)
                {
                    var misImgBtn = new MISLinkButton()
                    {
                        Content = (i + 1).ToString(),
                        Width = 35,
                        BorderThickness = new Thickness(1, 0, 0, 0),
                        Style = this.FindResource("DefaultLinkButton2Style") as Style
                    };
                    mCurrentPagers.Add((i + 1));
                    misImgBtn.Click += OnMisImgBtn_Click;
                    if (PART_Content != null)
                    {
                        PART_Content.Children.Add(misImgBtn);
                    }
                }
            }
            else
            {
                mPagerType = PagerType.Complex;
                for (var i = 0; i < 5; i++)
                {
                    var misImgBtn = new MISLinkButton() { Content = (i + 1).ToString(), Width = 35, BorderThickness = new Thickness(1, 0, 0, 0), Style = this.FindResource("DefaultLinkButton2Style") as Style };
                    misImgBtn.Click += OnMisImgBtn_Click;
                    if (i.Equals(0))
                    {
                        misImgBtn.Tag = 0;
                    }
                    if (i.Equals(4))
                    {
                        misImgBtn.Tag = 5;
                    }
                    mCurrentPagers.Add((i + 1));
                    if (PART_Content != null)
                    {
                        PART_Content.Children.Add(misImgBtn);
                    }
                }
                PART_Content.Children.Add(new MISLinkButton() { Content = "...", Width = 35, BorderThickness = new Thickness(1, 0, 0, 0), Style = this.FindResource("DefaultLinkButton3Style") as Style });
                PART_Content.Children.Add(new MISLinkButton() { Content = PageCount.ToString(), Width = 35, BorderThickness = new Thickness(1, 0, 0, 0), Style = this.FindResource("DefaultLinkButton2Style") as Style });
            }
            SetLinkButtonFocus(0);
            _SetNextpageAndPreviouspageState();
            if (PART_Previouspage != null)
            {
                PART_Previouspage.Click += OnPART_Previouspage_Click;
            }
            if (PART_Nextpage != null)
            {
                PART_Nextpage.Click += OnPART_Nextpage_Click;
            }
        }

        /// <summary>
        /// 当前DataGrid显示的数据总条数，用于计算页码数
        /// </summary>
        public Int32 Total
        {
            get
            {
                return (Int32)GetValue(TotalProperty);
            }
            set
            {
                SetValue(TotalProperty, value);
            }
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register("Total", typeof(Int32), typeof(MISPager), new PropertyMetadata(0));

        /// <summary>
        /// 每页显示条数
        /// </summary>
        public Int32 PageSize
        {
            get
            {
                return (Int32)GetValue(PageSizeProperty);
            }
            set
            {
                SetValue(PageSizeProperty, value);
            }
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(Int32), typeof(MISPager), new PropertyMetadata(10));


        /// <summary>
        /// 页码索引
        /// </summary>
        public Int32 PageIndex
        {
            get
            {
                return (Int32)GetValue(PageIndexProperty);
            }
            set
            {
                SetValue(PageIndexProperty, value);
            }
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(Int32), typeof(MISPager), new FrameworkPropertyMetadata(1));


        /// <summary>
        /// 页码数
        /// </summary>
        public Int32 PageCount
        {
            get
            {
                return (Int32)GetValue(PageCountProperty);
            }
            set
            {
                SetValue(PageCountProperty, value);
            }
        }

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(Int32), typeof(MISPager), new PropertyMetadata(0));

        public static readonly RoutedEvent PageChangedEvent = EventManager.RegisterRoutedEvent("PageChanged",
            RoutingStrategy.Bubble, typeof(EventHandler<PageChangedEventArgs>), typeof(MISPager));


        public event EventHandler<PageChangedEventArgs> PageChanged
        {
            add
            {
                AddHandler(PageChangedEvent, value);
            }
            remove
            {
                RemoveHandler(PageChangedEvent, value);
            }
        }


        /// <summary>
        /// 计算当前选中的分页按钮的索引
        /// </summary>
        private Int32 CalculationCurrentSelectPagerButtonWithIndex()
        {
            return mCurrentPagers.FindIndex((o) =>
            {
                return o == PageIndex;
            });
        }
        /// <summary>
        /// 维护当前分页控件显示的页码数据
        /// </summary>
        /// <param name="addSubtract"></param>
        private void _MaintainCurrentPagers(AddSubtract addSubtract)
        {
            if (addSubtract == AddSubtract.Add)
            {
                for (var i = 0; i < mCurrentPagers.Count; i++)
                {
                    mCurrentPagers[i] = mCurrentPagers[i] + 1;
                }
            }
            if (addSubtract == AddSubtract.subtract)
            {
                for (var i = 0; i < mCurrentPagers.Count; i++)
                {
                    mCurrentPagers[i] = mCurrentPagers[i] - 1;
                }
            }
        }
        /// <summary>
        /// 下一页
        /// </summary>
        private void OnPART_Nextpage_Click(object sender, RoutedEventArgs e)
        {
            var _index = CalculationCurrentSelectPagerButtonWithIndex() + 1;
            PageIndex++;
            _SetNextpageAndPreviouspageState();
            if (mPagerType == PagerType.Complex)
            {
                if (_index == 4)
                {
                    if (PageIndex == PageCount - 1)
                    {
                        PART_Nextpage.IsEnabled = false;
                    }
                    if (!mCurrentIsAddEllipsisCtrl)
                    {
                        mCurrentIsAddEllipsisCtrl = true;
                        PART_Content.Children.Insert(0, new MISLinkButton() { Content = "...", Width = 35, BorderThickness = new Thickness(1, 0, 0, 0), Style = this.FindResource("DefaultLinkButton3Style") as Style });
                    }
                    _RefreshPager(AddSubtract.Add);
                    _MaintainCurrentPagers(AddSubtract.Add);
                }
                else
                {
                    SetLinkButtonFocus(_index);
                }
            }
            else
            {
                SetLinkButtonFocus(_index);
            }
        }
        /// <summary>
        /// 上一页
        /// </summary>
        private void OnPART_Previouspage_Click(object sender, RoutedEventArgs e)
        {
            var _index = CalculationCurrentSelectPagerButtonWithIndex() - 1;
            PageIndex--;
            _SetNextpageAndPreviouspageState();
            if (mPagerType == PagerType.Complex)
            {
                if (PageIndex == 1)
                {
                    if (mCurrentIsAddEllipsisCtrl)
                    {
                        mCurrentIsAddEllipsisCtrl = false;
                        PART_Content.Children.RemoveAt(0);
                        SetLinkButtonFocus(0);
                    }
                    return;
                }
                if (_index == 0)
                {
                    _RefreshPager(AddSubtract.subtract);
                    _MaintainCurrentPagers(AddSubtract.subtract);
                }
                else
                {
                    SetLinkButtonFocus(_index);
                }
            }
            else
            {
                SetLinkButtonFocus(_index);
            }
        }

        private void SetLinkButtonFocus(Int32 index)
        {
            if (mCurrentIsAddEllipsisCtrl)
            {
                PART_Content.Children[index + 1].Focus();
            }
            else
            {
                PART_Content.Children[index].Focus();
            }
        }

        protected virtual void OnPageChanged()
        {
            var eventArgs = new PageChangedEventArgs(PageIndex) { RoutedEvent = PageChangedEvent, Source = this };
            RaiseEvent(eventArgs);
        }

        private void _RefreshPager(AddSubtract addSubtract)
        {
            if (PART_Content.Children.Count > 0)
            {
                var _index = 0;
                var _contentCount = PART_Content.Children.Count;
                if (mCurrentIsAddEllipsisCtrl)
                {
                    _index = 1;
                    _contentCount = _contentCount - 1;
                }
                for (var i = 0; i < _contentCount - 2; i++)
                {
                    var misLinkBtn = PART_Content.Children[_index] as MISLinkButton;
                    if (misLinkBtn != null)
                    {
                        misLinkBtn.Content = addSubtract == AddSubtract.Add ? (Convert.ToInt32(misLinkBtn.Content) + 1).ToString() : (Convert.ToInt32(misLinkBtn.Content) - 1).ToString();
                    }
                    _index++;
                }
                if (addSubtract == AddSubtract.Add)
                {
                    PART_Content.Children[_index - 2].Focus();
                }
                else
                {
                    if (mCurrentIsAddEllipsisCtrl)
                    {
                        PART_Content.Children[2].Focus();
                    }
                    else
                    {
                        PART_Content.Children[1].Focus();
                    }
                }
            }
        }

        /// <summary>
        /// 设置上一页下一页按钮显示状态
        /// </summary>
        private void _SetNextpageAndPreviouspageState()
        {
            if (PageIndex == 1)
            {
                PART_Previouspage.IsEnabled = false;
            }
            if (PageIndex > 1)
            {
                PART_Previouspage.IsEnabled = true;
                PART_Nextpage.IsEnabled = true;
            }
            if (mPagerType == PagerType.Complex)
            {
                if (PageIndex == PageCount - 1)
                {
                    PART_Previouspage.IsEnabled = true;
                    PART_Nextpage.IsEnabled = false;
                }
            }
            else
            {
                if (PageIndex == PageCount)
                {
                    PART_Previouspage.IsEnabled = true;
                    PART_Nextpage.IsEnabled = false;
                }
            }
            PART_PageIndex.Text = PageIndex.ToString();
        }
        /// <summary>
        /// 页码索引点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMisImgBtn_Click(object sender, RoutedEventArgs e)
        {
            var misImgBtn = sender as MISLinkButton;
            PageIndex = Convert.ToInt32(misImgBtn.Content);
            _SetNextpageAndPreviouspageState();
            if (mPagerType == PagerType.Complex)
            {
                _RefreshPager(misImgBtn);
            }
            OnPageChanged();
        }

        private void _RefreshPager(MISLinkButton misImgBtn)
        {
            if (misImgBtn.Tag != null)
            {
                if (misImgBtn.Tag.Equals(0))
                {
                    if (PageIndex > 1)
                    {
                        if (PageIndex == 2 && mCurrentIsAddEllipsisCtrl)
                        {
                            mCurrentIsAddEllipsisCtrl = false;
                            PART_Content.Children.RemoveAt(0);
                        }
                        _RefreshPager(AddSubtract.subtract);
                        _MaintainCurrentPagers(AddSubtract.subtract);
                    }
                }
                if (misImgBtn.Tag.Equals(5))
                {
                    if (!mCurrentIsAddEllipsisCtrl)
                    {
                        mCurrentIsAddEllipsisCtrl = true;
                        PART_Content.Children.Insert(0, new MISLinkButton() { Content = "...", Width = 35, BorderThickness = new Thickness(1, 0, 0, 0), Style = this.FindResource("DefaultLinkButton3Style") as Style });
                    }
                    _RefreshPager(AddSubtract.Add);
                    _MaintainCurrentPagers(AddSubtract.Add);
                }
            }
        }
    }

    /// <summary>
    /// 分页事件参数
    /// </summary>
    public class PageChangedEventArgs : RoutedEventArgs
    {
        public int PageIndex { get;
            set; }

        public PageChangedEventArgs(int pageIndex)
            : base()
        {
            PageIndex = pageIndex;
        }
    }

    /// <summary>
    /// 分页控件类型
    /// </summary>
    public enum PagerType
    {
        /// <summary>
        /// 复杂
        /// </summary>
        Complex,
        /// <summary>
        /// 默认
        /// </summary>
        Default
    }

    public enum AddSubtract
    {
        Add,
        subtract
    }
}
