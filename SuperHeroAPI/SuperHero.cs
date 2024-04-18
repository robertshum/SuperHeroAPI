namespace SuperHeroAPI
{
    public class SuperHero
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Description {  get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;

        // Navigation property for the join table SuperHeroPower
        public virtual ICollection<SuperHeroPower> SuperHeroPowers { get; set; } = new List<SuperHeroPower>();
    }
}
