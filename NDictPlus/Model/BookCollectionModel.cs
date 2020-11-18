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
            var convertedBooks = new Dictionary<string, Trie<DescriptionModel>>();
            Compatibility.Legacy.TryLoadInto(
                convertedBooks,
                description =>
                new DescriptionModel { new SingleDescriptionModel { Meaning = description } });
            foreach ((var name, var trie) in convertedBooks)
            {
                bookModels.Add(name, new BookModel(trie));
            }

            System.AppDomain.CurrentDomain.ProcessExit += (sender, e) => Save();
        }

        public void Save()
        {
            
        }
    }
}
