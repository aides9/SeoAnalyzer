using HtmlAgilityPack;
using SeoAnalyzer.Business.Exceptions;
using SeoAnalyzer.Business.Interfaces;
using SeoAnalyzer.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SeoAnalyzer.Business
{
    public class ParsingService: IParsingService
    {

        public IEnumerable<ParsingResult> Parsing(ParsingModel inputModel)
        {
            string[] words;

            var rawTexts = ParsingInput(inputModel.Text, inputModel.Options);
                
            words = GetWords(rawTexts, inputModel.Options);

            words = FilterStopWords(words, inputModel.Options);

            return GetWordsFrequency(words, inputModel.Options);
        }

        public string ParsingInput(string url, Options options)
        {
            var htmlDoc = new HtmlDocument();
            var web = new HtmlWeb();
            if (options.IsUrl)
            {
                try
                {
                    htmlDoc = web.Load(url);
                }
                catch (Exception)
                {
                    throw new InvalidUrlException(url);
                }
            }
            else
            {
                htmlDoc.LoadHtml(url);
            }

            var sb = new StringBuilder();
            var nodes = htmlDoc.DocumentNode.Descendants().Where(n =>
                n.NodeType == HtmlNodeType.Text &&
                !n.ParentNode.Name.Equals(Constants.Tags.Script) &&
                !n.ParentNode.Name.Equals(Constants.Tags.Style));

            foreach (var node in nodes)
            {
                sb.AppendLine(node.InnerText.Trim());
            }

            if (options.IncludeExternalLinks && htmlDoc.DocumentNode.SelectNodes("//a[@href]") != null)
            {
                foreach (var link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    var hrefValue = link.GetAttributeValue(Constants.Tags.Href, string.Empty);
                    if (!link.GetAttributeValue(Constants.Tags.Href, string.Empty).Contains(url) || !options.IsUrl)
                    {
                        sb.AppendLine(hrefValue);
                    }
                }
            }

            if (options.IncludeMetaData && htmlDoc.DocumentNode.SelectNodes("//meta/@content") != null)
            {
                foreach (var node in htmlDoc.DocumentNode.SelectNodes("//meta/@content"))
                {
                    sb.AppendLine(node.GetAttributeValue(Constants.Tags.Content, ""));
                }
            }
            return sb.ToString().Trim();

        }

        public string[] FilterStopWords(string[] inputs, Options options)
        {
            if (options.UseStopWords)
            {
                foreach (var stopWord in Constants.StopWords.EN)
                {
                    inputs = inputs.Where(word=>!word.Equals(stopWord)).ToArray();
                }
            }
            return inputs;
        }

        public List<ParsingResult> GetWordsFrequency(string[] inputs, Options options)
        {
            return inputs.GroupBy(word => word).Select(group => new ParsingResult { Word = group.Key, Frequency = options.CalculateFrequency ? group.Count():0 } ).ToList();
        }

        public string ReplaceExternalLinks(string input)
        {
            input = Regex.Replace(input,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "externallink");
            return input;
        }

        public string[] ExtractWords(string input)
        {
            var matches = Regex.Matches(input, @"\b[\w']*\b").Cast<Match>();

            return matches.Where(m => !string.IsNullOrEmpty(m.Value))
            .Select(m => TrimSuffix(m.Value.ToLower())).ToArray();
        }

        public string[] GetWords(string htmlWords, Options options)
        {
            if (options.IncludeExternalLinks)
            {
                htmlWords = ReplaceExternalLinks(htmlWords);
            }
            return ExtractWords(htmlWords);
        }

        private string TrimSuffix(string word)
        {
            var apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
    }
}
