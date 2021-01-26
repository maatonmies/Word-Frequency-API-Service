using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WordFrequencyService.Utilities
{
    public static class Extensions
    {
        public static string Filter(this string str, string regex, string replacement)
        {
            return Regex.Replace(str, regex, replacement);
        }

        public static bool DictionaryEquals(this IDictionary<string, int> @this, IDictionary<string, int> other)
        {
            return @this.Count == other.Count
                   && @this.All(kvp => other.ContainsKey(kvp.Key)
                                       && @this[kvp.Key].Equals(other[kvp.Key]));
        }
    }
}
