using System;
using System.Collections.Generic;
using System.Text;

namespace WordFrequencyService.Contract
{
    public class WordOccurrenceInfo
    {
        public DateTime FirstDateOfOccurrence { get; set; }
        public string FirstOccurrenceUrl { get; set; }
        public DateTime LastDateOfOccurrence { get; set; }
        public string LastOccurrenceUrl { get; set; }
    }
}
