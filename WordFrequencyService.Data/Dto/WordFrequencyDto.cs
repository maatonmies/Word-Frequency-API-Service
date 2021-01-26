using System;
using System.Collections.Generic;
using System.Text;

namespace WordFrequencyService.Data.Dto
{
    public class WordFrequencyDto
    {
        public string Word { get; set; }
        public int Frequency { get; set; }
        public string Url { get; set; }
    }
}
