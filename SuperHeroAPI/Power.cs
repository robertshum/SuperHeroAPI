using System.Text.Json.Serialization;

namespace SuperHeroAPI
{
    public class Power
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [JsonIgnore]
        public virtual ICollection<SuperHero> SuperHeroes { get; set; } = new List<SuperHero>(); 
    }
}
