using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;
using WordFrequencyService.Utilities;

[assembly: InternalsVisibleTo("WordFrequencyService.Test.Unit")]
namespace WordFrequencyService.Core.Services
{
    public class ContentFetcher : IContentFetcher
    {
        private const string HtmlMarkup = @"(<script(\s|\S)*?<\/script>)|(<style(\s|\S)*?<\/style>)|(<!--(\s|\S)*?-->)|(<\/?(\s|\S)*?>)";
        private const string NonLetters = @"[^a-zA-Z]";
        private const string MultipleWhiteSpaces = @"\s+";

        public async Task<string> FetchStringContentFromUrl(string url)
        {
            var uri = new Uri(url);
            var restClient = new RestClient(uri.GetLeftPart(UriPartial.Authority));
            var request = new RestRequest(uri.PathAndQuery);
            var rawContent = (await restClient.ExecuteGetAsync(request)).Content;
            var cleanContent = GetCleanContent(rawContent);

            return cleanContent;
        }

        internal string GetCleanContent(string rawContent)
        {
            return rawContent
                .Replace("\n", "")
                .Replace("nbsp", "")
                .Filter(HtmlMarkup, " ")
                .Filter(NonLetters, " ")
                .Filter(MultipleWhiteSpaces, " ");
        }
    }
}
