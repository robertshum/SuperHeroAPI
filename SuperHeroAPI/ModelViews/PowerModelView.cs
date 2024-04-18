using System.Text.Json.Serialization;

namespace SuperHeroAPI.ModelViews
{
    public class PowerModelView
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
