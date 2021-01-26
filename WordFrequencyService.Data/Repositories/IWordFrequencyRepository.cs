using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WordFrequencyService.Data.Dto;
using WordFrequencyService.Data.Entities;

namespace WordFrequencyService.Data.Repositories
{
    public interface IWordFrequencyRepository
    {
        public Task<IEnumerable<WordFrequencyData>> SaveWordFrequencyData(IEnumerable<WordFrequencyDto> wordFrequencies);
        public Task<IEnumerable<WordFrequencyData>> GetTopHundredWordFrequencyData();
    }
}