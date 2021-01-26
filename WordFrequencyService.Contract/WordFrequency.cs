using System;

namespace WordFrequencyService.Contract
{
    public class WordFrequency
    {
        public string Word { get; set; }
        public int TotalFrequency { get; set; }
        public int LatestFrequency { get; set; }
        public decimal RelativeFrequency { get; set; }
        public WordOccurrenceInfo OccurrenceInfo { get; set; }
    }
}
