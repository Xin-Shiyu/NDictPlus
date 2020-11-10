using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
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
        PhraseQueryModel currentModel = null;
        string currentBookName = string.Empty;
        string queryWord;
        UIStates _uiState; // never use this directly!

        private bool TryOpenBook(string bookName)
        {
            if (bookName == string.Empty)
            {
                currentModel = null; // closes the book
                currentBookName = string.Empty;
                UIState = UIStates.BookSelection;
                return true;
            }
            else if (bookName != currentBookName)
            {
                bool opened = 
                    bookCollectionModel.bookModels.TryGetValue(bookName, out var properModel);
                if (opened)
                {
                    currentModel = properModel;
                    currentBookName = bookName;
                    Result =
                        new LazyLoadCollection<PartialPhraseViewModel>(
                            new ObservableCollectionMapper<
                                KeyValuePair<string, DescriptionModel>,
                                PartialPhraseViewModel>(
                                properModel.Result,
                                pair =>
                                {
                                    (var phrase, var model) = pair;
                                    var first = model?[0];
                                    return new PartialPhraseViewModel
                                    {
                                        Phrase = phrase,
                                        PartOfSpeech = first?.PartOfSpeech,
                                        Description = first?.Meaning,
                                        LeftCount = model.Count - 1
                                    };
                                }
                            ));
                }
                return opened;
            }
            return false;
        }

        public IEnumerable<BookViewModel> BookList { get; private set; }

        public UIStates UIState
        {
            get => _uiState;

            private set
            {
                if (_uiState == value) return;
                _uiState = value;
                RaisePropertyChanged("UIState");
            }
        }

        public ICommand OpenBookCommand { get; private set; }
        public ICommand VisitPhraseCommand { get; private set; }

        public MainViewModel()
        {
            bookCollectionModel = new BookCollectionModel();
            bookCollectionModel.Load();
            BookList =
                new ObservableCollectionMapper
                <KeyValuePair<string, PhraseQueryModel>, BookViewModel>
                (
                    bookCollectionModel.bookModels,
                    model => new BookViewModel
                    {
                        Name = model.Key,
                        EntryCount = model.Value.PhraseCount
                    }
                );

            VisitPhraseCommand =
                new DelegateCommand<string>(
                    phrase =>
                    {
                        UIState = UIStates.PhraseDisplay;
                    }
                    );
            OpenBookCommand =
                new DelegateCommand<string>(
                    bookName =>
                    {
                        if (TryOpenBook(bookName))
                        {
                            UIState = UIStates.PhraseQuery;
                        }
                    }
                    );
        }

        public IEnumerable<PartialPhraseViewModel> Result { get; private set; }

        // IEnumerator<KeyValuePair<string, DescriptionModel>> phraseEnumerator;

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
