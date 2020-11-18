using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NDictPlus.Compatibility
{
    public static class Legacy
    {
        public static void TryLoadInto<TTarget, TDictionary>(
            IDictionary<string, TDictionary> target,
            Func<string, TTarget> mapper)
            where TDictionary : IDictionary<string, TTarget>, new()
        {
            var programPath = AppDomain.CurrentDomain.GetAssemblies()[0].Location;
            var booksPath = Path.Combine(programPath, "phrasebooks");
            var booksBase = new DirectoryInfo(booksPath);
            if (!booksBase.Exists) return;
            var booksDir = 
                booksBase.GetDirectories("*", searchOption: SearchOption.TopDirectoryOnly);

            foreach (var bookDir in booksDir)
            {
                var bookName = bookDir.Name;
                _ = target.TryAdd(bookName, new TDictionary());
                var convertedBook = target[bookName];

                var oldBook = new VersionedDictionary(bookDir.FullName);
                foreach ((var word, var description) in oldBook)
                {
                    convertedBook.TryAdd(word, mapper(description));
                }
            }

            booksBase.MoveTo(booksPath + ".old");
        }
    }
}
