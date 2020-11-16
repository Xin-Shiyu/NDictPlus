using System.Collections.ObjectModel;

namespace NDictPlus.Model
{
    public class UsageExampleModel
    {
        public string Usage { get; set; } = string.Empty;

        public string Meaning { get; set; } = string.Empty;
    }

    public class SingleDescriptionModel
    {
        public string PartOfSpeech { get; set; } = string.Empty;

        public string Pronunciation { get; set; } = string.Empty;

        public string Meaning { get; set; } = string.Empty;

        public ObservableCollection<UsageExampleModel> Examples { get; set; }
            = new ObservableCollection<UsageExampleModel>();

        public ObservableCollection<string> RelatedPhrases { get; set; }
            = new ObservableCollection<string>();
    }

    public class DescriptionModel : ObservableCollection<SingleDescriptionModel> {}
}
