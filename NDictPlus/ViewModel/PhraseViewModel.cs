using NDictPlus.Model;
using NDictPlus.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NDictPlus.ViewModel
{
    public class UsageExampleViewModel : NotifyPropertyChangedBase
    {
        readonly UsageExampleModel _model;

        public UsageExampleViewModel(UsageExampleModel model)
        {
            _model = model;
        }

        public string Usage
        {
            get => _model.Usage;

            set
            {
                _model.Usage = value;
                RaisePropertyChanged("Usage");
            }
        }

        public string Meaning
        {
            get => _model.Meaning;

            set
            {
                _model.Meaning = value;
                RaisePropertyChanged("Meaning");
            }
        }
    }

    public class SingleDescriptionViewModel : NotifyPropertyChangedBase
    {
        readonly SingleDescriptionModel _model;
        private ICommand addExampleCommand;
        private ICommand addRelatedPhraseCommand;
        private IEnumerable<UsageExampleViewModel> _examples;

        public SingleDescriptionViewModel(SingleDescriptionModel model)
        {
            System.Diagnostics.Debug.WriteLine(model.Meaning);
            _model = model;
            Examples =
                ObservableCollectionMapper.Map(
                    model.Examples,
                    model => new UsageExampleViewModel(model));
            AddExampleCommand =
                new StatedDelegateCommand(
                    () =>
                    {
                        model.Examples.Add(new UsageExampleModel());
                    });
        }

        public string PartOfSpeech
        {
            get => _model.PartOfSpeech;

            set
            {
                _model.PartOfSpeech = value;
                RaisePropertyChanged("PartOfSpeech");
            }
        }

        public string Pronunciation
        {
            get => _model.Pronunciation;

            set
            {
                _model.Pronunciation = value;
                RaisePropertyChanged("Pronunciation");
            }
        }

        public string Meaning
        {
            get => _model.Meaning;

            set
            {
                _model.Meaning = value;
                RaisePropertyChanged("Meaning");
            }
        }

        public IEnumerable<UsageExampleViewModel> Examples
        {
            get => _examples;

            set
            {
                if (_examples is IDisposable disposable) disposable?.Dispose();
                _examples = value;
            }
        }

        public ObservableCollection<string> RelatedPhrases
        {
            // the type is so trivial that there is no magic
            get => _model.RelatedPhrases;
            // there is no need to listen to the event because as the view binds,
            // it actually gets the ref to the real collection in the real model
            // and it should handle all these event firing stuffs and so on
        }

        public ICommand AddExampleCommand
        {
            get => addExampleCommand;

            private set
            {
                addExampleCommand = value;
                RaisePropertyChanged("AddExampleCommand");
            }
        }

        public ICommand AddRelatedPhraseCommand
        {
            get => addRelatedPhraseCommand;

            private set
            {
                addRelatedPhraseCommand = value;
                RaisePropertyChanged("AddRelatedPhraseCommand");
            }
        }
    }

    public class PhraseViewModel : NotifyPropertyChangedBase, IDisposable
    {
        private ICommand addDescriptionCommand;
        private IEnumerable<SingleDescriptionViewModel> _descriptions;

        public string Phrase { get; private set; }

        public IEnumerable<SingleDescriptionViewModel> Descriptions
        {
            get => _descriptions;

            private set
            {
                if (_descriptions is IDisposable disposable) disposable?.Dispose();
                _descriptions = value;
            }
        }

        public PhraseViewModel(string phrase, DescriptionModel model)
        {
            System.Diagnostics.Debug.WriteLine("starts");
            Phrase = phrase;
            Descriptions =
                ObservableCollectionMapper.Map(
                    model,
                    model => new SingleDescriptionViewModel(model));
            AddDescriptionCommand =
                new StatedDelegateCommand(() =>
                {
                    model.Add(new SingleDescriptionModel());
                });
        }

        public ICommand AddDescriptionCommand
        {
            get => addDescriptionCommand;

            private set
            {
                addDescriptionCommand = value;
                RaisePropertyChanged("AddDescriptionCommand");
            }
        }

        public void Dispose()
        {
            Descriptions = null;
        }
    }
}
