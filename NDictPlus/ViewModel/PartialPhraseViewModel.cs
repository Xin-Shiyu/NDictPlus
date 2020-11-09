using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NDictPlus.ViewModel
{
    public class PartialPhraseViewModel
    {
        public string Phrase { get; set; }
        public string PartOfSpeech { get; set; }
        public string Description { get; set; }
        public int LeftCount { get; set; }
    }
}