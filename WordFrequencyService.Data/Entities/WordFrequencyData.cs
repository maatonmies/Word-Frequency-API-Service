using System;
using System.Collections.Generic;
using System.Text;

namespace WordFrequencyService.Data.Entities
{
    public class WordFrequencyData
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public int Frequency { get; set; }
        public string Url { get; set; }
        public DateTime InsertDate { get; set; }
        public int TotalFrequency { get; set; }

    }
}
