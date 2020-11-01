using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace NDictPlus.Model
{
    public class PhraseDescription : INotifyPropertyChanged
    {
        private string partOfSpeech;
        private string pronunciation;
        private string description;

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

        public string Description
        {
            get => description;

            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }

        public ObservableCollection<string> Examples { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class DescriptionModel : ObservableCollection<PhraseDescription> {}
}
