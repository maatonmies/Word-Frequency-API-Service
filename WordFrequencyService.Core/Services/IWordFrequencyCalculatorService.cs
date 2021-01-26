using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordFrequencyService.Contract;

namespace WordFrequencyService.Core.Services
{
    public interface IWordFrequencyCalculatorService
    {
        Task<IDictionary<string, int>> CalculateAndStoreTopHundredWordByFrequencyFromUrl(string url);
        Task<IOrderedEnumerable<WordFrequency>> GetTopHundredMostFrequentlyUsedWords();
    }
}
