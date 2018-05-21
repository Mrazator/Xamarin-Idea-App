using System;
using System.Collections.Generic;
using System.Text;

namespace IdeaApp.Models
{
    public class Ideas
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public string UserId { get; set; }
    }
}
