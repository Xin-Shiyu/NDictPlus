using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using NDictPlus.Model;
using NDictPlus.Utilities;

namespace NDictPlus.ViewModel
{
    public enum UIStates
    { 
        BookSelection,
        BookCreation,
        PhraseQuery,
        PhraseCreation,
        PhraseDisplay,
    }

    public class MainViewModel : NotifyPropertyChangedBase
    {
        readonly BookCollectionModel bookCollectionModel;
        WordQueryModel currentModel = null;
        string queryWord;
        UIStates uiState;

        private bool TryOpenBook(string bookName)
        {
            if (bookName == "")
            {
                currentModel = null; // closes the book;
                return true;
            }
            else
            {
                bool opened = 
                    bookCollectionModel.bookModels.TryGetValue(bookName, out var targetModel);
                if (opened) currentModel = targetModel;
                return opened;
            }
        }

        public IEnumerable<string> BookNameList { get; private set; }

        public UIStates UIState
        {
            get => uiState;

            private set
            {
                uiState = value;
                RaisePropertyChanged("UIState");
            }
        }

        public MainViewModel()
        {
            bookCollectionModel = new BookCollectionModel();
            bookCollectionModel.Load();
            BookNameList =
                new ObservableCollectionMapper<KeyValuePair<string, WordQueryModel>, string>
                (bookCollectionModel.bookModels, x => x.Key);
        }

        public ObservableCollection<PartialPhraseViewModel> Result { get; private set; }

        public void TryLoadMoreResult()
        {
            
        }

        public string QueryWord
        {
            get => queryWord;

            set
            {
                queryWord = value;

            }
        }
    }
}
