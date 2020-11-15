﻿using NDictPlus.Model;
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
        PhraseDisplay,
    }

    public class MainViewModel : NotifyPropertyChangedBase
    {
        readonly BookCollectionModel bookCollectionModel;
        BookModel currentModel = null;

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
                        CreateLazyCollection(properModel);

                    LoadMoreResultCommand =
                        new StatedDelegateCommand(lazyCollection.LoadMore);

                    Result = lazyCollection;
                    UIState = UIStates.PhraseQuery;
                }
                return opened;
            }
            return false;
        }

        private static LazyLoadCollection<PartialPhraseViewModel> CreateLazyCollection(BookModel bookModel) 
            => 
            new LazyLoadCollection<PartialPhraseViewModel>(
                new ObservableCollectionMapper<KeyValuePair<string, DescriptionModel>, PartialPhraseViewModel>(
                    bookModel.QueryModel,
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

        public MainViewModel()
        {
            bookCollectionModel = new BookCollectionModel();
            bookCollectionModel.Load();
            BookList =
                new ObservableCollectionMapper
                <KeyValuePair<string, BookModel>, BookViewModel>
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
                    () => CreatePhrase(QueryString),
                    () => IsValidQuery && !HasExactMatch());

            ShortcutCommand =
                new StatedDelegateCommand(
                    () =>
                    {
                        if (!IsValidQuery) return;

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

        public IEnumerable<PartialPhraseViewModel> Result
        {
            get => _result;
            private set
            {
                _result = value;
                RaisePropertyChanged("Result");
            }
        }

        private bool IsValidQuery { get => !string.IsNullOrEmpty(QueryString); }

        private bool HasExactMatch(bool IsOnly = false)
        {
            if (Result == null) return false;

            var enumerator = Result.GetEnumerator();
            if (!enumerator.MoveNext()) return false;

            var first = enumerator.Current.Phrase;

            if (IsOnly)
            {
                var hasSecond = enumerator.MoveNext();
                return first == QueryString && !hasSecond;
            }
            else
            {
                return first == QueryString;
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
                        currentModel.QueryModel.Query(value);

                        if (HasExactMatch(IsOnly: true))
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
                _currentPhraseDetail = value;
                RaisePropertyChanged("CurrentPhraseDetail");
            }
        }
    }
}
