using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Nativa;

namespace NDictPlus.Model
{
    class BookCollectionModel
    {
        public readonly Dictionary<string, WordQueryModel> bookModels =
                new Dictionary<string, WordQueryModel>();

        public void Load()
        {
            bookModels.Add(
                "french",
                new WordQueryModel(
                    new Trie<DescriptionModel>()
                    {
                        {
                            "pomme",
                            new DescriptionModel
                            {
                                new PhraseDescription
                                {
                                    Meaning = "apple",
                                    PartOfSpeech = "n.f.",
                                    Examples = new ObservableCollection<UsageExample>
                                    {
                                        new UsageExample
                                        {
                                            Usage = "J'aime manger les pommes.",
                                            Meaning = "I like eating apples."
                                        }
                                    }
                                }
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
                new WordQueryModel(
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
