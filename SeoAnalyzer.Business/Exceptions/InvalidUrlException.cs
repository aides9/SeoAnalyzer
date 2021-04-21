using System;

namespace SeoAnalyzer.Business.Exceptions
{
    public class InvalidUrlException: Exception
    {
      public InvalidUrlException(string url): base(String.Format($"Invalid URL: {url}")) { }
    }

}
