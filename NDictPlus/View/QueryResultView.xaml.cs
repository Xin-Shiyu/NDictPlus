using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using NDictPlus.Utilities;
using System.Reflection.PortableExecutable;

namespace NDictPlus.View
{
    public partial class QueryResultView : Page
    {
        public QueryResultView()
        {
            InitializeComponent();
        }

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button)
                if (DataContext.GetType()
                    .GetProperty("VisitPhraseCommand")
                    .GetValue(DataContext) is ICommand command)
                    command.Execute(string.Empty);
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ViewportHeight + e.VerticalOffset + 5 >= e.ExtentHeight)
            {
                if (DataContext
                    .GetType()
                    .GetProperty("LoadMoreResultCommand")
                    .GetValue(DataContext) is ICommand command)
                    command.Execute(null);
            }
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    class DescriptionLeftCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value switch
            {
                -1 => "STILL EMPTY",
                0 => string.Empty,
                1 => $"1+ SENSE",
                int count => $"{count}+ SENSES",
                _ => throw new ArgumentException("value")
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }

    [ValueConversion(typeof(int), typeof(Visibility))]
    class LeftCountVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value switch
            {
                -1 => Visibility.Collapsed,
                _ => Visibility.Visible
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => null;
    }
}
