namespace SuperHeroAPI
{
    public class SuperHeroPower
    {
        //Join between power and superhero
        public int PowerId { get; set; }
        public required Power Power { get; set; }
        public int SuperHeroId { get; set; }
        public required SuperHero SuperHero { get; set; }
    }
}
