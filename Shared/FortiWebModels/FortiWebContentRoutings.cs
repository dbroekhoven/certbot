﻿using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutings
    {
        [JsonPropertyName("results")]
        public List<FortiWebContentRoutingResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
