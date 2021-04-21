

namespace SeoAnalyzer.Business.Models
{
    public class Options
    {
        public bool IsUrl { get; set; }
        public bool UseStopWords { get; set; }
        public bool CalculateFrequency { get; set; }
        public bool IncludeMetaData { get; set; }
        public bool IncludeExternalLinks { get; set; }
    }
}
