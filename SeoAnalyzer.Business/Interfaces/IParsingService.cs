
using SeoAnalyzer.Business.Models;
using System.Collections.Generic;

namespace SeoAnalyzer.Business.Interfaces
{
    public interface IParsingService
    {
        IEnumerable<ParsingResult> Parsing(ParsingModel inputModel);
        string ParsingInput(string url, Options options);
        string[] FilterStopWords(string[] inputs, Options options);
        List<ParsingResult> GetWordsFrequency(string[] inputs, Options options);
        string[] ExtractWords(string input);
        string[] GetWords(string htmlWords, Options options);
        string ReplaceExternalLinks(string input);
    }
}
