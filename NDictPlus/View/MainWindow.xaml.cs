using Nativa;
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

namespace NDictPlus
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //*
            var view = new View.PhraseDetailView();
            view.Phrase.Text = "pomme";
            var model = new Model.BookCollectionModel();
            model.Load();
            var book = model
                .bookModels["french"];
            book.QueryWord = "pomme";
            var enumerator = book.Result.GetEnumerator();
            enumerator.MoveNext();
            var item = enumerator.Current;
            var descriptions = item.Value;
            view
                .Descriptions
                .ItemsSource = descriptions;
            ViewFrame.Content = view;
            //*/
            /*
            var view = new View.QueryResultView();
            var model = new Model.BookCollectionModel();
            model.Load();
            var book = model.bookModels["french"];
            view.DataContext = book;
            QueryBox.DataContext = book;
            var binding = 
                new Binding("QueryWord") 
                { 
                    Mode = BindingMode.TwoWay,
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                };
            QueryBox.SetBinding(TextBox.TextProperty, binding);
            ViewFrame.Content = view;
            */
        }
    }
}
