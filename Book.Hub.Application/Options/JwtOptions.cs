namespace Books.Hub.Appliction.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string LfeTimeInDays { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;

    }
}
