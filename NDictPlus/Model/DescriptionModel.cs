using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NDictPlus.Model
{
    public class UsageExample : INotifyPropertyChanged
    {
        private string usage;
        private string meaning;

        public string Usage
        {
            get => usage; 
            
            set
            {
                usage = value;
                RaisePropertyChanged("Usage");
            }
        }

        public string Meaning
        {
            get => meaning; 
            
            set
            {
                meaning = value;
                RaisePropertyChanged("Meaning");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class PhraseDescription : INotifyPropertyChanged
    {
        private string partOfSpeech;
        private string pronunciation;
        private string meaning;

        public string PartOfSpeech
        {
            get => partOfSpeech;

            set
            {
                partOfSpeech = value;
                RaisePropertyChanged("PartOfSpeech");
            }
        }

        public string Pronunciation
        {
            get => pronunciation;

            set
            {
                pronunciation = value;
                RaisePropertyChanged("Pronunciation");
            }
        }

        public string Meaning
        {
            get => meaning;

            set
            {
                meaning = value;
                RaisePropertyChanged("Meaning");
            }
        }

        public ObservableCollection<UsageExample> Examples { get; set; }

        public ObservableCollection<string> RelatedPhrases { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class DescriptionModel : ObservableCollection<PhraseDescription> {}
}
