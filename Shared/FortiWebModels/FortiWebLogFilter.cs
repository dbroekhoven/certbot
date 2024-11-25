using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebLogFilter
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("logic")]
        public FortiWebLogFilterLogic Logic { get; set; }

        [JsonPropertyName("value")]
        public List<string> Value { get; set; }
    }

    public class FortiWebLogFilterComparison
    {
        [JsonPropertyName("timestamp")]
        public bool IsTimestamp { get; set; }

        [JsonPropertyName("string")]
        public bool? IsString { get; set; }

        [JsonPropertyName("enum")]
        public bool? IsEnum { get; set; }
    }

    public class FortiWebLogFilterLogic
    {
        [JsonPropertyName("is")]
        public FortiWebLogFilterComparison Is { get; set; }

        [JsonPropertyName("search")]
        public string Search { get; set; }

        [JsonPropertyName("splitter")]
        public string Splitter { get; set; }

        [JsonPropertyName("RANGE")]
        public int Range { get; set; }

        [JsonPropertyName("modifiers")]
        public List<string> Modifiers { get; set; }

        [JsonPropertyName("NOT")]
        public int? Not { get; set; }
    }
}
