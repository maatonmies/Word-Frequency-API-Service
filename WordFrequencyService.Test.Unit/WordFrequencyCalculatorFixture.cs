using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using WordFrequencyService.Core.Services;
using WordFrequencyService.Data.Dto;
using WordFrequencyService.Data.Repositories;
using WordFrequencyService.Utilities;

namespace WordFrequencyService.Test.Unit
{
    [TestFixture]
    public class WordFrequencyCalculatorFixture
    {
        private static object[] _testCases =
        {
            new object[]
            {   "https://www.testcase1.com/",
                "one two three four five one two two two four three three one one two",
                new Dictionary<string, int>
                {
                    {"two", 5 },
                    {"one", 4 },
                    {"three", 3 },
                    {"four", 2 },
                    {"five", 1 }
                }},
            new object[]
            {
                "https://www.testcase2.com/",
                "apple banana x y mango apple a pear pear apple v banana banana mango banana banana pear pear",
                new Dictionary<string, int>
                {
                    {"banana", 5 },
                    {"pear", 4 },
                    {"apple", 3 },
                    {"mango", 2 }
                }},
            new object[]
            {
                "https://www.testcase3.com/",
                "beta r r k alpha alpha x x x",
                new Dictionary<string, int>
                {
                    {"alpha", 2 },
                    {"beta", 1 }
                }}
        };

        private IWordFrequencyCalculatorService GetWordFrequencyCalculatorService(string content)
        {
            var mockContentFetcher = new Mock<IContentFetcher>();
            mockContentFetcher
                .Setup(mcf => mcf.FetchStringContentFromUrl(It.IsAny<string>()))
                .ReturnsAsync(content);

            var mockRepository = new Mock<IWordFrequencyRepository>();
            mockRepository.Setup(mr => mr.SaveWordFrequencyData(It.IsAny<IEnumerable<WordFrequencyDto>>()));

            return new WordFrequencyCalculatorService(mockRepository.Object, mockContentFetcher.Object);
        }

        [Test]
        [TestCaseSource(nameof(_testCases))]
        public async Task WordFrequencyIsCorrectlyCalculatedFromContent(string url, string content, Dictionary<string, int> expected)
        {
            var service = GetWordFrequencyCalculatorService(content);
            var actual = await service.CalculateAndStoreTopHundredWordByFrequencyFromUrl(url);

            Assert.That(actual.DictionaryEquals(expected));

            Console.WriteLine($"Expected: {JsonConvert.SerializeObject(expected)}");
            Console.WriteLine($"Actual: {JsonConvert.SerializeObject(actual)}");
        }
    }
}
