using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace PV239_IdeaApp
{
	public class Ideas
	{
        [Version]
        public string Version { get; set; }

	    [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

	    [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

	    [JsonProperty(PropertyName = "IsFavorite")]
        public bool IsFavorite { get; set; }

	    [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

	    [JsonProperty(PropertyName = "UserId")]
        public string UserId { get; set; }
    }
}

