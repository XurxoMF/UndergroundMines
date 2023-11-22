namespace UndergroundMines
{
    public class Config
    {
        /// <summary>
        /// Y level where mines will be generated.<br/>1 = Sea level.<br/>0 = Mantle.<br/>0.48(default) = In a regular world is Y-54.
        /// </summary>
        public double yLevel = ModStaticConfig.DefaultHeight;

        /// <summary>
        /// Generate or not aditional ores in the mines.<br/>true = Generate aditionar ores.<br/>false = Don't generate aditional ores.
        /// </summary>
        public bool aditionalOres = true;

        /// <summary>
        /// Generate or not mushrooms in the mines.<br/>true = Generate mushrooms.<br/>false = Don't generate mushrooms.
        /// </summary>
        public bool addMushrooms = true;

        /// <summary>
        /// Generate or not locust nests in the mines.<br/>true = Generate locust nests.<br/>false = Don't generate locust nests.
        /// </summary>
        public bool addLocustNests = true;

        /// <summary>
        /// How many entrances the mod will generate.<br/>0 = No entrances.<br/>1 = All the UndergroundCross with no water in the top layer will have entrance.
        /// </summary>
        public double entranceChance = ModStaticConfig.DefaultEntrances;

        /// <summary>
        /// How many new mines will be generated in chunks with no colindant mines.<br/>0 = 0 new mines.<br/>1 = Every chunk will have a mine XD
        /// </summary>
        public double newMineSpawnChance = ModStaticConfig.DeffaultNewMineSpawnChance;

        public Config()
        {
        }
    }
}