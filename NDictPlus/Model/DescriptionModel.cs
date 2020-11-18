using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Linq;
using System;

namespace NDictPlus.Model
{
    public class UsageExampleModel
    {
        private string usage = string.Empty;
        private string meaning = string.Empty;

        private int Version { get; set; } = -2; // currently
        public string Usage
        {
            get => usage;
            set
            {
                usage = value;
                ++Version;
            }
        }
        public string Meaning 
        { 
            get => meaning;
            set
            {
                meaning = value;
                ++Version;
            }
        }
    }

    public class SingleDescriptionModel
    {
        private string partOfSpeech = string.Empty;
        private string pronunciation = string.Empty;
        private string meaning = string.Empty;

        private int Version { get; set; } = -3; // currently
        public string PartOfSpeech
        {
            get => partOfSpeech;
            set
            {
                partOfSpeech = value;
                ++Version;
            }
        }
        public string Pronunciation
        {
            get => pronunciation;
            set
            {
                pronunciation = value;
                ++Version;
            }
        }
        public string Meaning 
        { 
            get => meaning;
            set
            {
                meaning = value;
                ++Version;
            }
        }

        public ObservableCollection<UsageExampleModel> Examples { get; }
            = new ObservableCollection<UsageExampleModel>();

        public ObservableCollection<string> RelatedPhrases { get; }
            = new ObservableCollection<string>();

        public SingleDescriptionModel()
        {
            // increases the version when collections changed.
            Examples.CollectionChanged += (sender, e) => ++Version;
            RelatedPhrases.CollectionChanged += (sender, e) => ++Version;
        }
    }

    public class DescriptionModel : ObservableCollection<SingleDescriptionModel> {} 
}
