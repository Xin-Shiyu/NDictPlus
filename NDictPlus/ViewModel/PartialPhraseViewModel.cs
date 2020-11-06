using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace NDictPlus.ViewModel
{
    class PartialPhraseViewModel
    {
        public string Phrase { get; set; }
        public string PartOfSpeech { get; set; }
        public string Description { get; set; }
        public int LeftCount { get; set; }
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