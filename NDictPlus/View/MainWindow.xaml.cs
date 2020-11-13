using NDictPlus.View;
using NDictPlus.ViewModel;
using System;
using System.Windows;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;

namespace NDictPlus
{
    public partial class MainWindow : Window
    {
        readonly MainViewModel mainViewModel;
        readonly Dictionary<UIStates, Page> views;
        
        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();

            views = new Dictionary<UIStates, Page>
            {
                { UIStates.BookSelection, new BookListView() },
                { UIStates.PhraseDisplay, new PhraseDetailView() },
                { UIStates.PhraseQuery, new QueryResultView() },
            };

            this.DataContext = mainViewModel;
            foreach (Page view in views.Values) 
            {
                view.DataContext = mainViewModel;
            }

            mainViewModel.PropertyChanged += ViewModel_PropertyChanged;

            PageContainer.Content = views[UIStates.BookSelection];
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UIState":
                    PageContainer.Content = views[mainViewModel.UIState];
                    break;
                default:
                    break;
            }
        }

        private void PageContainer_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            PageContainer.RemoveBackEntry(); 
            // dirty but the possibly only way to prevent keyboard navigation
        }
    }
}
