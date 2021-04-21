using Microsoft.AspNetCore.Mvc;
using SeoAnalyzer.Business.Exceptions;
using SeoAnalyzer.Business.Interfaces;
using SeoAnalyzer.Business.Models;
using SeoAnalyzer.Models;
using System;
using System.Diagnostics;

namespace SeoAnalyzer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IParsingService _seoParser;
        public HomeController(IParsingService seoParser)
        {
            _seoParser = seoParser;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Result(HomeViewModel homeViewModel)
        {
            try
            {
                ModelState.Clear();
                var parsingModel = new ParsingModel
                {
                    Text = homeViewModel.Text,
                    Options = new Options { 
                        IsUrl = homeViewModel.IsUrl,
                        UseStopWords = homeViewModel.UseStopWords,
                        CalculateFrequency = homeViewModel.CalculateFrequency,
                        IncludeMetaData = homeViewModel.IncludeMetaData,
                        IncludeExternalLinks = homeViewModel.IncludeExternalLinks
                    }
                };
                var result = _seoParser.Parsing(parsingModel);
                return View("Result", result);
            }
            catch (InvalidUrlException urlException)
            {
                ModelState.AddModelError("", urlException.Message);
                return View("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "We have an error. " + ex.Message);
                return View("Index");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
