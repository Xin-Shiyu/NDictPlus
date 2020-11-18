using NDictPlus.Model;
using NDictPlus.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace NDictPlus.ViewModel
{
    public enum UIStates
    { 
        BookSelection,
        BookCreation,
        PhraseQuery,
        PhraseDisplay,
    }

    public class MainViewModel : NotifyPropertyChangedBase
    {
        readonly BookCollectionModel bookCollectionModel;
        BookModel currentModel = null;
        BookModel.TrieQueryModel<DescriptionModel> currentQueryModel = null;

        // never use theses directly!
        string _currentBookName = string.Empty;
        string _queryString;
        UIStates _uiState;
        private IEnumerable<BookViewModel> _bookList;
        private IEnumerable<PartialPhraseViewModel> _result;
        private PhraseViewModel _currentPhraseDetail;
        private ICommand _openBookCommand;
        private ICommand _visitPhraseCommand;
        private ICommand _loadMoreResultCommand;
        private ICommand _createPhraseCommand;
        private ICommand _shortcutCommand;

        public IEnumerable<BookViewModel> BookList
        {
            get => _bookList;
            private set
            {
                if (_bookList is IDisposable disposable) disposable?.Dispose();
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
        public string QueryString
        {
            get => _queryString;

            set
            {
                _queryString = value;
                switch (UIState)
                {
                    case UIStates.PhraseQuery:
                    {
                        currentQueryModel.Query(value);

                        if (HasExactMatch(isOnly: true))
                        {
                            VisitPhrase(value);
                        }
                        else
                        {
                            (CreatePhraseCommand as CuriousDelegateCommand)
                                .UpdateExecutability();
                        }

                        break;
                    }
                    case UIStates.PhraseDisplay:
                    {
                        UIState = UIStates.PhraseQuery;
                        goto case UIStates.PhraseQuery;
                    }
                    default:
                        break;
                }
                RaisePropertyChanged("QueryString");
            }
        }
        public IEnumerable<PartialPhraseViewModel> Result
        {
            get => _result;
            private set
            {
                if (_result is IDisposable disposable) disposable?.Dispose();
                _result = value;
                RaisePropertyChanged("Result");
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
        public PhraseViewModel CurrentPhraseDetail
        {
            get => _currentPhraseDetail;

            set
            {
                if (_currentPhraseDetail is IDisposable disposable) disposable?.Dispose();
                _currentPhraseDetail = value;
                RaisePropertyChanged("CurrentPhraseDetail");
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
        public ICommand CreatePhraseCommand
        {
            get => _createPhraseCommand;
            private set
            {
                _createPhraseCommand = value;
                RaisePropertyChanged("CreatePhraseCommand");
            }
        }
        public ICommand ShortcutCommand
        {
            get => _shortcutCommand;
            private set
            {
                _shortcutCommand = value;
                RaisePropertyChanged("ShortcutCommand");
            }
        }

        private bool IsValidQuery() => !string.IsNullOrEmpty(QueryString);
        private bool HasExactMatch(bool isOnly = false)
        {
            if (Result == null) return false;

            var enumerator = Result.GetEnumerator();
            if (!enumerator.MoveNext()) return false;

            var first = enumerator.Current.Phrase;

            if (isOnly)
            {
                var hasSecond = enumerator.MoveNext();
                return first == QueryString && !hasSecond;
            }
            else
            {
                return first == QueryString;
            }
        }

        private static LazyLoadCollection<PartialPhraseViewModel> CreateLazyCollection(
            BookModel.TrieQueryModel<DescriptionModel> queryModel)
            =>
            LazyLoadCollection.From(
                ObservableCollectionMapper.Map(
                    queryModel,
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
                    }));

        private bool TryOpenBook(string bookName)
        {
            if (bookName == string.Empty)
            {
                CloseCurrentBook();
                return true;
            }
            else if (bookName != CurrentBookName)
            {
                bool bookExists =
                    bookCollectionModel.bookModels.TryGetValue(bookName, out var properModel);
                if (bookExists)
                {
                    OpenBook(bookName, properModel);
                }
                return bookExists;
            }
            return false;
        }

        private void OpenBook(string bookName, BookModel properModel)
        {
            currentModel = properModel;
            CurrentBookName = bookName;
            currentQueryModel = properModel.NewQueryModel();

            var lazyCollection =
                CreateLazyCollection(currentQueryModel);

            LoadMoreResultCommand =
                new StatedDelegateCommand(lazyCollection.LoadMore);

            Result = lazyCollection;
            UIState = UIStates.PhraseQuery;
        }

        private void CloseCurrentBook()
        {
            currentModel = null; // closes the book
            CurrentBookName = string.Empty;
            CurrentPhraseDetail = null;
            UIState = UIStates.BookSelection;
            QueryString = string.Empty;
        }

        private void CreatePhrase(string phrase)
        {
            currentModel.Create(phrase);
            VisitPhrase(phrase);
        }
        private void VisitPhrase(string phrase)
        {
            CurrentPhraseDetail = new PhraseViewModel(phrase, currentModel.GetExactResult(phrase));
            UIState = UIStates.PhraseDisplay;
        }

        public MainViewModel()
        {
            bookCollectionModel = new BookCollectionModel();
            bookCollectionModel.Load();
            BookList =
                ObservableCollectionMapper.Map
                (
                    bookCollectionModel.bookModels,
                    model => new BookViewModel
                    {
                        Name = model.Key,
                        EntryCount = model.Value.PhraseCount
                    }
                );

            VisitPhraseCommand =
                new StatedDelegateCommand<string>(VisitPhrase);

            OpenBookCommand =
                new StatedDelegateCommand<string>(
                    bookName =>
                    {
                        _ = TryOpenBook(bookName);
                        // TO-DO: Handle situation when book does not exist
                    });

            CreatePhraseCommand =
                new CuriousDelegateCommand(
                    act: () => CreatePhrase(QueryString),
                    when: () => IsValidQuery() && !HasExactMatch());

            ShortcutCommand =
                new StatedDelegateCommand(
                    () =>
                    {
                        if (!IsValidQuery()) return;

                        if (HasExactMatch())
                        {
                            VisitPhrase(QueryString);
                        }
                        else
                        {
                            CreatePhrase(QueryString);
                        }
                    });
        }
    }
}
