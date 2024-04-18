﻿namespace SuperHeroAPI
{
    public class Power
    {
        public int Id { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation property for the join table SuperHeroPower
        public virtual ICollection<SuperHeroPower> SuperHeroPowers { get; set; } = new List<SuperHeroPower>();
    }
}
