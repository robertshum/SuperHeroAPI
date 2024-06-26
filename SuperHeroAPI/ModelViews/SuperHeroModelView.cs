﻿using System.Text.Json.Serialization;

namespace SuperHeroAPI.ModelViews
{
    public class SuperHeroModelView
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public List<int> PowerIds { get; set; } = new List<int>();
    }
}
