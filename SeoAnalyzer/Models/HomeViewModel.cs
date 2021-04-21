
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SeoAnalyzer.Models
{
    public class HomeViewModel
    {
        [Required]
        [DisplayName("Url or Text")]
        public string Text { get; set; }
        [DisplayName("Is Url")]
        public bool IsUrl { get; set; }
        [DisplayName("Use Stop Words")]
        public bool UseStopWords { get; set; }
        [DisplayName("Calculate Words Frequency")]
        public bool CalculateFrequency { get; set; }
        [DisplayName("Include Metadata")]
        public bool IncludeMetaData { get; set; }
        [DisplayName("Include External Links")]
        public bool IncludeExternalLinks { get; set; }
    }
}
