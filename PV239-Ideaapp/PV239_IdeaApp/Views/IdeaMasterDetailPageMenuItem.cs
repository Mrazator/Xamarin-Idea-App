using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV239_IdeaApp.Views
{

    public class IdeaMasterDetailPageMenuItem
    {
        public Type TargetType { get; set; }

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

        public IdeaMasterDetailPageMenuItem()
        {
            TargetType = typeof(IdeaMasterDetailPageDetail);
        }       
    }
}