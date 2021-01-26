using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WordFrequencyService.Core.Services
{
    public interface IContentFetcher
    {
        Task<string> FetchStringContentFromUrl(string url);
    }
}
