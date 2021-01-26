using System;
using System.Threading.Tasks;
using NUnit.Framework;
using WordFrequencyService.Core.Services;

namespace WordFrequencyService.Test.Unit
{
    [TestFixture]
    public class ContentFetcherFixture
    {
        private static ContentFetcher GetContentFetcher() => new ContentFetcher();

        [Test]
        public void NewLinesAreRemovedFromRawContent()
        {
            const string rawContent = "\n sample word";
            const string expected = " sample word";

            var actual = GetContentFetcher().GetCleanContent(rawContent);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NonBreakingSpaceIsRemovedFromRawContent()
        {
            const string rawContent = "nbsp sample word";
            const string expected = " sample word";

            var actual = GetContentFetcher().GetCleanContent(rawContent);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HtmlMarkUpIsRemovedFromRawContent()
        {
            const string rawContent = "<div>sample word</div><p>some other text</p><script>function(){ some javascript }</script>";
            const string expected = " sample word some other text ";

            var actual = GetContentFetcher().GetCleanContent(rawContent);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NonLettersAreRemovedFromRawContent()
        {
            const string rawContent = "123^&(_$$$ sample text";
            const string expected = " sample text";

            var actual = GetContentFetcher().GetCleanContent(rawContent);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void MultipleWhiteSpacesAreRemovedFromRawContent()
        {
            const string rawContent = "sample text                    sample text";
            const string expected = "sample text sample text";

            var actual = GetContentFetcher().GetCleanContent(rawContent);
            Assert.AreEqual(expected, actual);
        }
    }
}
