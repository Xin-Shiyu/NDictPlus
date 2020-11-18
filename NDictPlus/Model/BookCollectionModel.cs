using Nativa;
using NDictPlus.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace NDictPlus.Model
{
    class BookCollectionModel
    {
        public readonly IDictionary<string, BookModel> bookModels =
            new ObservableDictionary<string, BookModel>();
        private static readonly SharpYaml.Serialization.SerializerSettings
            serializerSettings =
            new SharpYaml.Serialization.SerializerSettings
            {
                EmitAlias = false,
                EmitTags = false
            };
        private static readonly SharpYaml.Serialization.Serializer serializer =
            new SharpYaml.Serialization.Serializer(serializerSettings);
        private static readonly string workingDirectoryPath =
            Directory.GetCurrentDirectory();
        private static readonly string booksDirectoryPath =
            Path.Combine(workingDirectoryPath, "phrasebooksy");
        private static readonly DirectoryInfo booksDirectory =
            new DirectoryInfo(booksDirectoryPath);
            
        public void Create(string bookName)
        {
            bookModels.Add(bookName, new BookModel());
        }

        public void Load()
        {
            #region Compatibility
            var convertedBooks = new Dictionary<string, Trie<DescriptionModel>>();
            Compatibility.Legacy.TryLoadInto(
                convertedBooks,
                description =>
                new DescriptionModel
                {
                new SingleDescriptionModel
                {
                    Meaning = description,
                    PartOfSpeech = "n/a"
                }
                });
            foreach ((var name, var trie) in convertedBooks)
            {
                bookModels.Add(name, new BookModel(trie));
            }
            #endregion
            // problems may arise if the old book overlaps with the new book
            // but I have no time to fix this now
            // this will not happen very often i guess right
            if (booksDirectory.Exists)
            {
                var files = booksDirectory.GetFiles();
                foreach (var file in files)
                {
                    var bookName = Path.GetFileNameWithoutExtension(file.Name);
                    using var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
                    var reader = new StreamReader(stream, System.Text.Encoding.Unicode);
                    var str = reader.ReadToEnd();
                    // var trie = serializer.Deserialize<Trie<DescriptionModel>>(reader);
                    var trie = serializer.Deserialize<Trie<DescriptionModel>>(str);
                    if (trie != null) bookModels.Add(bookName, new BookModel(trie));
                }
            }

            System.AppDomain.CurrentDomain.ProcessExit += (sender, e) => Save();
        }

        public void Save()
        {
            if (!booksDirectory.Exists) booksDirectory.Create();

            foreach ((var bookName, var model) in bookModels)
            {
                var trie = model.GetTrie();
                var filename = Path.Combine(booksDirectoryPath, bookName);
                using var stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                var writer = new StreamWriter(stream, System.Text.Encoding.Unicode);
                // serializer.Serialize(writer, trie);
                var str = serializer.Serialize(trie);
                writer.Write(str);
                writer.Flush();
                stream.Flush();
            }
        }
    }
}
