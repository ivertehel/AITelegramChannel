using Newtonsoft.Json;

namespace AiTelegramChannel.ServerHost.Unsplash.Models
{
    public class UnsplashResponse
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }

        [JsonProperty("results")]
        public List<UnsplashResult> Results { get; set; }
    }

    public class UnsplashResult
    {
        [JsonProperty("urls")]
        public Urls Urls { get; set; }
    }

    public class Urls
    {
        [JsonProperty("raw")]
        public string Raw { get; set; }

        [JsonProperty("full")]
        public string Full { get; set; }

        [JsonProperty("regular")]
        public string Regular { get; set; }

        [JsonProperty("small")]
        public string Small { get; set; }

        [JsonProperty("thumb")]
        public string Thumb { get; set; }

        [JsonProperty("small_s3")]
        public string SmallS3 { get; set; }
    }
}