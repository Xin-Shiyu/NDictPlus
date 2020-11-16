using NDictPlus.View;
using NDictPlus.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NDictPlus
{
    public partial class MainWindow : Window
    {
        public static readonly MainViewModel mainViewModel = new MainViewModel();
        public static readonly Dictionary<UIStates, Type> Views =
            new Dictionary<UIStates, Type>
            {
                { UIStates.BookSelection, typeof(BookListView) },
                { UIStates.PhraseDisplay, typeof(PhraseDetailView) },
                { UIStates.PhraseQuery, typeof(QueryResultView) },
            };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }

        private void PageContainer_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (sender is Frame frame) frame.RemoveBackEntry();
        }
    }

    [ValueConversion(typeof(UIStates), typeof(Page))]
    public class UIStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // for some unknown problem about Frame, this is the only solution I come up with
            // to solve the switch-existing-pages-twice-in-one-method problem
            var instance = Activator.CreateInstance(MainWindow.Views[(UIStates)value]);
            (instance as Page).DataContext = MainWindow.mainViewModel;
            return instance;
        }

        /*
         * 复现方式如下：
         * 有两个页面的实例，通过给 Frame 的 Content 属性赋值来实现页面的切换。
         * 假设两个类分别叫 Page1 和 Page2，实例分别是 p1, p2
         * 通常情况下两个页面是可以来回切换的，但是假如：
         * 1）最开始 Frame 的 Content 是 p2
         * 2）在一个方法中连续将 Frame 的 Content 依次设定为了 p1 p2
         * 最后 Frame 的内容会是 p1 而不是 p2。
         * 另外：
         * 假如监听了 Frame 的 Navigating 和 Navigated 事件，会发现在第一次切换的时候
         * Navigating 事件会被激活，但是第二次切换的时候不会。在整个方法结束后，Navigated 事件
         * 会被激活且仅激活一次，其 EventArgs 里面的 Content 是 p1。
         * 然而，假如不使用已有的 Page 实例，而是每次导航的时候 new 一个，
         * 则不会出现这种问题，最终 Frame 会导航到 Page2 上，并且 Page1 不会出现在导航的历史记录里。
         */
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
