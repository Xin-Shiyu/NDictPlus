using System.Collections.ObjectModel;

namespace NDictPlus.Model
{
    public class UsageExampleModel
    {
        public string Usage { get; set; }

        public string Meaning { get; set; }
    }

    public class SingleDescriptionModel
    {
        public string PartOfSpeech { get; set; }

        public string Pronunciation { get; set; }

        public string Meaning { get; set; }

        public ObservableCollection<UsageExampleModel> Examples { get; set; }

        public ObservableCollection<string> RelatedPhrases { get; set; }
    }

    public class DescriptionModel : ObservableCollection<SingleDescriptionModel> {}
}
