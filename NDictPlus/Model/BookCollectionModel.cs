using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Nativa;
using NDictPlus.Utilities;

namespace NDictPlus.Model
{
    class BookCollectionModel
    {
        public readonly IDictionary<string, PhraseQueryModel> bookModels =
            new ObservableDictionary<string, PhraseQueryModel>();

        public void Load()
        {
            bookModels.Add(
                "french",
                new PhraseQueryModel(
                    new Trie<DescriptionModel>()
                    {
                        {
                            "pomme",
                            new DescriptionModel
                            {
                                new PhraseDescription
                                {
                                    Pronunciation = "pom",
                                    Meaning = "apple",
                                    PartOfSpeech = "n.f.",
                                    Examples = new ObservableCollection<UsageExample>
                                    {
                                        new UsageExample
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
                            "pain",
                            new DescriptionModel
                            {
                                new PhraseDescription
                                {
                                    Meaning = "bread",
                                    PartOfSpeech = "n.m.",
                                    Examples = new ObservableCollection<UsageExample>
                                    {
                                        new UsageExample
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
                new PhraseQueryModel(
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
                    }));
        }

        public void Save()
        {

        }
    }
}
