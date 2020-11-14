using Nativa;
using NDictPlus.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NDictPlus.Model
{
    class BookCollectionModel
    {
        public readonly IDictionary<string, BookModel> bookModels =
            new ObservableDictionary<string, BookModel>();

        public void Load()
        {
            var ha = new Trie<DescriptionModel>();
            for (int i = 0; i < 100; ++i) ha.Add(i.ToString(), new DescriptionModel());
            bookModels.Add(
                "french",
                new BookModel(
                    new Trie<DescriptionModel>()
                    {
                        {
                            "pomme",
                            new DescriptionModel
                            {
                                new SingleDescriptionModel
                                {
                                    Pronunciation = "pom",
                                    Meaning = "apple",
                                    PartOfSpeech = "n.f.",
                                    Examples = new ObservableCollection<UsageExampleModel>
                                    {
                                        new UsageExampleModel
                                        {
                                            Usage = "J'aime manger les pommes.",
                                            Meaning = "I like eating apples."
                                        }
                                    },
                                    RelatedPhrases = new ObservableCollection<string>
                                    {
                                        "banane",
                                        "fraise"
                                    }
                                },
                            }
                        },
                        {
                            "haha",
                            new DescriptionModel()
                        },
                        {
                            "pain",
                            new DescriptionModel
                            {
                                new SingleDescriptionModel
                                {
                                    Meaning = "bread",
                                    PartOfSpeech = "n.m.",
                                    Examples = new ObservableCollection<UsageExampleModel>
                                    {
                                        new UsageExampleModel
                                        {
                                            Usage = "Les pains et les baguettes.",
                                            Meaning = "Bread and baguettes."
                                        }
                                    }
                                },
                                new SingleDescriptionModel
                                {
                                    Meaning = "bread",
                                    PartOfSpeech = "n.m.",
                                    Examples = new ObservableCollection<UsageExampleModel>
                                    {
                                        new UsageExampleModel
                                        {
                                            Usage = "Les pains et les baguettes.",
                                            Meaning = "Bread and baguettes."
                                        }
                                    }
                                }
                            }
                        },
                    }));
            bookModels.Add(
                "japanese",
                new BookModel(ha
                    /*
                    new Trie<DescriptionModel>()
                    {
                        {
                            "りんご",
                            new DescriptionModel
                            {
                                new PhraseDescription
                                {
                                    Meaning = "apple",
                                    PartOfSpeech = "n.",
                                    Examples = new ObservableCollection<UsageExample>
                                    {
                                        new UsageExample
                                        {
                                            Usage = "私はリンゴを食べるのが好きです。",
                                            Meaning = "I like eating apples."
                                        }
                                    }
                                }
                            }
                        },
                        {
                            "パン",
                            new DescriptionModel
                            {
                                new PhraseDescription
                                {
                                    Meaning = "bread",
                                    PartOfSpeech = "n.",
                                    Examples = new ObservableCollection<UsageExample>
                                    {
                                        new UsageExample
                                        {
                                            Usage = "パンとバゲット。",
                                            Meaning = "Bread and baguettes."
                                        }
                                    }
                                }
                            }
                        },
                    }*/));
        }

        public void Save()
        {

        }
    }
}
