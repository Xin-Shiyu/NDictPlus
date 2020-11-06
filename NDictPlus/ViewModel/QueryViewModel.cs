using System;
using System.Collections.Generic;
using System.Text;
using NDictPlus.Model;

namespace NDictPlus.ViewModel
{
    class QueryViewModel
    {
        BookCollectionModel bookCollection = new BookCollectionModel();

        public QueryViewModel()
        {
            bookCollection.Load();
        }

    }
}
