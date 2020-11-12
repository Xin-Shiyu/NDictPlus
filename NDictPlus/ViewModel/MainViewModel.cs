using NDictPlus.Model;
using NDictPlus.Utilities;
using System.Collections.Generic;
using System.Windows.Input;

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

        // never use theses directly!
        string _currentBookName = string.Empty;
        string _queryString;
        UIStates _uiState;
        private IEnumerable<BookViewModel> _bookList;
        private IEnumerable<PartialPhraseViewModel> _result;
        private ICommand _openBookCommand;
        private ICommand _visitPhraseCommand;
        private ICommand _loadMoreResultCommand;

        private bool TryOpenBook(string bookName)
        {
            if (bookName == string.Empty)
            {
                currentModel = null; // closes the book
                CurrentBookName = string.Empty;
                UIState = UIStates.BookSelection;
                return true;
            }
            else if (bookName != _currentBookName)
            {
                bool opened =
                    bookCollectionModel.bookModels.TryGetValue(bookName, out var properModel);
                if (opened)
                {
                    currentModel = properModel;
                    CurrentBookName = bookName;
                    var lazyCollection =
                        new LazyLoadCollection<PartialPhraseViewModel>(
                            new ObservableCollectionMapper<
                                KeyValuePair<string, DescriptionModel>,
                                PartialPhraseViewModel>(
                                properModel.Result,
                                pair =>
                                {
                                    (var phrase, var model) = pair;
                                    var count = model.Count;
                                    var first = count != 0 ? model[0] : null;
                                    return new PartialPhraseViewModel
                                    {
                                        Phrase = phrase,
                                        PartOfSpeech = first?.PartOfSpeech,
                                        Description = first?.Meaning,
                                        LeftCount = count - 1
                                    };
                                }
                            ));
                    LoadMoreResultCommand =
                        new DelegateCommand(lazyCollection.LoadMore);
                    Result = lazyCollection;
                    UIState = UIStates.PhraseQuery;
                }
                return opened;
            }
            return false;
        }

        public IEnumerable<BookViewModel> BookList
        {
            get => _bookList;
            private set
            {
                _bookList = value;
                RaisePropertyChanged("BookList");
            }
        }

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

        public ICommand OpenBookCommand
        {
            get => _openBookCommand;
            private set
            {
                _openBookCommand = value;
                RaisePropertyChanged("OpenBookCommand");
            }
        }
        public ICommand VisitPhraseCommand
        {
            get => _visitPhraseCommand;
            private set
            {
                _visitPhraseCommand = value;
                RaisePropertyChanged("VisitPhraseCommand");
            }
        }

        public ICommand LoadMoreResultCommand
        {
            get => _loadMoreResultCommand;
            private set
            {
                _loadMoreResultCommand = value;
                RaisePropertyChanged("LoadMoreResultCommand");
            }
        }

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
                    });

            OpenBookCommand =
                new DelegateCommand<string>(
                    bookName =>
                    {
                        _ = TryOpenBook(bookName);
                        // TO-DO: Handle situation when book does not exist
                    });
        }

        public IEnumerable<PartialPhraseViewModel> Result
        {
            get => _result;
            private set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        // IEnumerator<KeyValuePair<string, DescriptionModel>> phraseEnumerator;

        public string QueryString
        {
            get => _queryString;

            set
            {
                _queryString = value;
                if (currentModel != null)
                {
                    currentModel.QueryPhrase = QueryString;
                }
                RaisePropertyChanged("QueryString");
            }
        }

        public string CurrentBookName
        {
            get => _currentBookName;

            set
            {
                _currentBookName = value;
                RaisePropertyChanged("CurrentBookName");
            }
        }
    }
}
