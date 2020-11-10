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

namespace NDictPlus.View
{
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
                return $"{(int)value} more descriptions";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
