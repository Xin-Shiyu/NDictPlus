﻿using NDictPlus.View;
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
        MainViewModel mainViewModel;
        Dictionary<UIStates, Page> views;
        
        public MainWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel();

            views = new Dictionary<UIStates, Page>
            {
                { UIStates.BookSelection, new BookListView()},
                { UIStates.PhraseDisplay, new PhraseDetailView()},
                { UIStates.PhraseQuery, new QueryResultView()},
                { UIStates.PhraseCreation, new PhraseCreationView()},
            };

            foreach (Page view in views.Values) 
            {
                view.DataContext = mainViewModel;
            }

            mainViewModel.PropertyChanged += ViewModel_PropertyChanged;

            ViewFrame.Content = new BookListView() { DataContext = mainViewModel };
        }

        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UIState":
                    ViewFrame.Content = views[mainViewModel.UIState];
                    break;
                default:
                    break;
            }
        }
    }
}
