using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WordFrequencyService.Data.Contexts;
using WordFrequencyService.Data.Dto;
using WordFrequencyService.Data.Entities;
using WordFrequencyService.Data.Sql;

namespace WordFrequencyService.Data.Repositories
{
    public class WordFrequencySqlRepository : IWordFrequencyRepository
    {
        private readonly IDbContextFactory<WordFrequencySqlDbContext> _contextFactory;

        public WordFrequencySqlRepository(IDbContextFactory<WordFrequencySqlDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<IEnumerable<WordFrequencyData>> SaveWordFrequencyData(IEnumerable<WordFrequencyDto> wordFrequencies)
        {
            await using var context = _contextFactory.CreateDbContext();
            var data = (await Task.WhenAll(wordFrequencies
                    .Select(async wf => new WordFrequencyData
                    {
                        Word = wf.Word.Trim(),
                        Frequency = wf.Frequency,
                        Url = wf.Url.Trim(),
                        TotalFrequency = await GetTotalFrequency(wf.Word.Trim(), wf.Frequency)
                    })))
                .ToList();

            context.AddRange(data);
            await context.SaveChangesAsync();
            return data;
        }

        public async Task<IEnumerable<WordFrequencyData>> GetTopHundredWordFrequencyData()
        {
            await using var context = _contextFactory.CreateDbContext();

            var words = await context
                .WordFrequencies
                .FromSqlRaw(WordFrequencySql.GetAllRecordsForTopHundredWords)
                .ToListAsync();

            return words;
        }

        private async Task<int> GetTotalFrequency(string word, int frequency)
        {
            await using var context = _contextFactory.CreateDbContext();
            var existingRecords = await context.WordFrequencies.Where(wf => wf.Word == word).ToListAsync();
            return existingRecords.Any() ? existingRecords.Sum(er => er.Frequency) + frequency : frequency;
        }
    }
}
