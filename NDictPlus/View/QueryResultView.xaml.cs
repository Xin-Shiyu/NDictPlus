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

namespace NDictPlus.View
{
    /// <summary>
    /// QueryResultView.xaml 的交互逻辑
    /// </summary>
    public partial class QueryResultView : Page
    {
        public QueryResultView()
        {
            InitializeComponent();
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    class DescriptionLeftCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value > 0)
            {
                return string.Empty;
            }
            else
            {
                return $"还有 {(int)value} 种解释";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
