using NUnit.Framework;
using SeoAnalyzer.Business.Exceptions;
using SeoAnalyzer.Business.Interfaces;
using SeoAnalyzer.Business.Models;

namespace SeoAnalyzer.Business.Test
{
    public class ParsingServiceTest
    {
        private IParsingService _parsingService;

        [SetUp]
        public void Setup()
        {
            _parsingService = new ParsingService();
        }

        [Test]
        public void GetWords_TextString_ReturnsSplitString()
        {
            var testString = "aaa bbb ccc ddd eee";
            string[] expected = { "aaa", "bbb", "ccc", "ddd", "eee" };
            Assert.AreEqual(_parsingService.GetWords(testString, new Options { IncludeExternalLinks = false }), expected);
        }

        [Test]
        public void SetLinks_TextLink_ReturnsExternalLink()
        {
            string testString = "aaa bbb ccc https://www.google.com/ ddd eee";
            string expected = "aaa bbb ccc externallink ddd eee";
            Assert.AreEqual(_parsingService.ReplaceExternalLinks(testString), expected);
        }


        [TestCase(new string[] { "aaa", "bbb", "ccc", "aaa" }, 2)]
        [TestCase(new string[] { "aaa", "aaa", "ddd", "aaa" }, 3)]
        public void CalculateFrequency_TurnOn_ReturnCalculatedFrequency(string[] testArray, int result)
        {
            Assert.AreEqual(_parsingService.GetWordsFrequency(testArray, new Options { CalculateFrequency = true })[0].Frequency, result);
        }

        [TestCase(new string[]{ "aaa", "bbb", "ccc", "aaa" }, 0)]
        [TestCase(new string[]{ "aaa", "aaa", "ddd", "aaa" }, 0)]
        public void CalculateFrequency_TurnOff_ReturnZeroFrequency(string[] testArray, int result)
        {
            Assert.AreEqual(_parsingService.GetWordsFrequency(testArray, new Options { CalculateFrequency = false })[0].Frequency, result);
        }

        [Test]
        public void FilterStopWords_StopWordsString_ReturnFilteredString()
        {
            string[] testArray = { "aaa", "a", "my", "happy", "ccc" };
            string[] expected = { "aaa","happy", "ccc" };

            Assert.AreEqual(_parsingService.FilterStopWords(testArray, new Options { UseStopWords = true }), expected);
        }

        [Test]
        public void ParsingInput_TextString_ReturnText()
        {
            var testHtml = @"<!DOCTYPE html>
                <html>
                <body>
                <h1> This is <b> bold </b> heading </h1>
                <p> This is <u> underlined </u> paragraph </p>
                <h2> This is <i> italic </i> heading </h2>
                </body>
                </html> ";
            Assert.IsTrue(_parsingService.ParsingInput(testHtml, new Options {}).GetType() == typeof(string));
        }

        [Test]
        public void ParsingInput_WrongUrl_ThrowInvalidUrlException()
        {
            var testUrl = @"http://www.yahoomix.com/";

            Assert.Throws<InvalidUrlException>(() => _parsingService.ParsingInput(testUrl, new Options { IsUrl = true }));
        }
    }
}