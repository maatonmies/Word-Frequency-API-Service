using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WordFrequencyService.Contract;
using WordFrequencyService.Core.Services;

namespace WordFrequencyService.Web.Controllers

{
    [ApiController]
    [Route("api/word-frequency")]
    public class WordFrequencyController : ControllerBase
    {
        private readonly IWordFrequencyCalculatorService _wordFrequencyCalculator;

        public WordFrequencyController(IWordFrequencyCalculatorService wordFrequencyCalculator)
        {
            _wordFrequencyCalculator = wordFrequencyCalculator;
        }

        /// <summary>
        /// Fetches content from url supplied and stores the top 100 most frequent words extracted from the page in a MSSQL database.
        /// </summary>
        /// <param name="url"> Url to extract words from. Will be executed as GET asynchronously to fetch contents so POST/PUT/DELETE endpoints won't work.
        /// All javascript and markup gets filtered out. Content-rich html pages work best.
        /// </param>
        /// <returns> Returns top 100 words stored in the database as dictionary of word and frequency ordered by the most frequent word towards to the less frequent word.</returns>
        /// <response code = "201"> Returns the words and frequencies extracted and stored. </response>
        /// <response code = "500"> If anything goes wrong the actual error message is returned. </response>
        [HttpPost]
        [Route("calculate-from-url")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CalculateAndStoreWordFrequencyFromUrl([FromQuery]string url)
        {
            return CreatedAtAction(nameof(CalculateAndStoreWordFrequencyFromUrl), 
                await _wordFrequencyCalculator.CalculateAndStoreTopHundredWordByFrequencyFromUrl(url));
        }

        /// <summary>
        /// Gets top hundred most frequent words from the MSSQL database. Run "calculate-from-url" POST endpoint above with a valid a url first to get results.
        /// </summary>
        /// <returns> The list of WordFrequencies ordered by the most frequent word towards to the less frequent word.</returns>
        /// <response code = "200"> Returns the top 100 words by freqency. </response>
        /// <response code = "500"> If anything goes wrong the actual error message is returned. </response>
        [HttpGet]
        [Route("top-hundred")]
        public async Task<IOrderedEnumerable<WordFrequency>> GetTopHundredMostFrequentWords()
        {
            return await _wordFrequencyCalculator.GetTopHundredMostFrequentlyUsedWords();
        }

    }
}
