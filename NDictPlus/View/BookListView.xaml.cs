using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace NDictPlus.View
{
    public partial class BookListView : Page
    {
        public BookListView()
        {
            InitializeComponent();
        }

        private void OnItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
                if (DataContext.GetType()
                    .GetProperty("OpenBookCommand")
                    .GetValue(DataContext) is ICommand command)
                    command.Execute(button.DataContext.GetType()
                                                      .GetProperty("Name")
                                                      .GetValue(button.DataContext));
        }
    }

    [ValueConversion(typeof(int), typeof(string))]
    class EntryCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            =>
            value switch
            {
                0 => "EMPTY",
                int count => $"{count} ENTRIES",
                _ => null
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
