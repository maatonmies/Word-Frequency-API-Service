using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordFrequencyService.Contract;
using WordFrequencyService.Data.Dto;
using WordFrequencyService.Data.Repositories;

namespace WordFrequencyService.Core.Services
{
    public class WordFrequencyCalculatorService : IWordFrequencyCalculatorService
    {
        private readonly IWordFrequencyRepository _repository;
        private readonly IContentFetcher _contentFetcher;

        public WordFrequencyCalculatorService(IWordFrequencyRepository repository, IContentFetcher contentFetcher)
        {
            _repository = repository;
            _contentFetcher = contentFetcher;
        }
        public async Task<IDictionary<string, int>> CalculateAndStoreTopHundredWordByFrequencyFromUrl(string url)
        {
            var content = await _contentFetcher.FetchStringContentFromUrl(url);

            var words = content
                .Split(" ")
                .Where(word => word.Length > 1)
                .GroupBy(word => word.ToLower())
                .ToArray();

            var topHundredWordsByFrequency =
                words
                .Select(group => new WordFrequencyDto
                {
                    Word = group.Key,
                    Frequency = group.Count(),
                    Url = url
                })
                .OrderByDescending(wordFrequency => wordFrequency.Frequency)
                .Take(100)
                .ToArray();

            await _repository.SaveWordFrequencyData(topHundredWordsByFrequency);

            var dictionary = topHundredWordsByFrequency
                .ToDictionary(word => word.Word, word => word.Frequency);

            return dictionary;
        }

        public async Task<IOrderedEnumerable<WordFrequency>> GetTopHundredMostFrequentlyUsedWords()
        {
            var data = (await _repository.GetTopHundredWordFrequencyData()).ToArray();

            if(!data.Any())
                return new List<WordFrequency>().OrderBy(wf => 1);

            var topWords = data.GroupBy(d => d.Word).ToArray();
            var totalOccurrences = topWords.Select(group => group.Max(wf => wf.TotalFrequency)).Sum();

            var results = topWords
                .Select(group =>
                {
                    var orderedRecords = group.OrderBy(g => g.Id).ToArray();
                    var firstRecord = orderedRecords.First();
                    var lastRecord = orderedRecords.Last();

                    return new WordFrequency
                    {
                        Word = group.Key,
                        TotalFrequency = lastRecord.TotalFrequency,
                        LatestFrequency = lastRecord.Frequency,
                        RelativeFrequency = Math.Round(lastRecord.TotalFrequency / ((decimal)totalOccurrences / 100), 4),
                        OccurrenceInfo = new WordOccurrenceInfo
                        {
                            FirstDateOfOccurrence = firstRecord.InsertDate,
                            FirstOccurrenceUrl = firstRecord.Url,
                            LastDateOfOccurrence = lastRecord.InsertDate,
                            LastOccurrenceUrl = lastRecord.Url
                        }
                    };
                });

            return results.OrderByDescending(r => r.TotalFrequency);
        }
    }
}