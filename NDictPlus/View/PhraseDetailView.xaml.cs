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

namespace NDictPlus.View
{
    /// <summary>
    /// PhraseDetailView.xaml 的交互逻辑
    /// </summary>
    public partial class PhraseDetailView : Page
    {
        public PhraseDetailView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button a)
            {
                MessageBox.Show(a.Tag as string);
            }
        }
    }
}
