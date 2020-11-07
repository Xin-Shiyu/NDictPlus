using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NDictPlus.Model;

namespace NDictPlus.ViewModel
{
    class QueryViewModel
    {
        readonly BookCollectionModel bookCollection = new BookCollectionModel();

        public QueryViewModel()
        {
            bookCollection.Load();
        }
    }
}
