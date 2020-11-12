using Accessibility;
using NDictPlus.Model;
using NDictPlus.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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

        public SingleDescriptionViewModel(SingleDescriptionModel model)
        {
            _model = model;
            Examples =
                new ObservableCollectionMapper<UsageExampleModel, UsageExampleViewModel>
                (model.Examples, model => new UsageExampleViewModel(model));
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

        public IEnumerable<UsageExampleViewModel> Examples { get; set; }

        public ObservableCollection<string> RelatedPhrases 
        {
            // the type is so trivial that there is no magic
            get => _model.RelatedPhrases;
            // there is no need to listen to the event because as the view binds,
            // it actually gets the ref to the real collection in the real model
            // and it should handle all these event firing stuffs and so on
            set
            {
                _model.RelatedPhrases = value;
                RaisePropertyChanged("RelatedPhrases");
                // i'm just not so sure about the view
                // this won't cost much huh
            }
        }
    }

    class PhraseViewModel
    {
        public string Phrase { get; private set; }

        public IEnumerable<SingleDescriptionViewModel> Descriptions { get; private set; }

        public PhraseViewModel(string phrase, DescriptionModel model)
        {
            Phrase = phrase;
            Descriptions =
                new ObservableCollectionMapper<SingleDescriptionModel, SingleDescriptionViewModel>
                (model, model => new SingleDescriptionViewModel(model));
        }
    }
}
