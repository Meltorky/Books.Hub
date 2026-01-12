using Imagekit.Constant;
using System.ComponentModel.DataAnnotations;

namespace Books.Hub.Appliction.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
       
        [Range(0.1, 24, ErrorMessage = "Lifetime must be between 6 minutes and 24 Hours.")]
        public double LfeTimeInDays { get; set; }
    }
}
