namespace UndergroundMines
{
    public class Config
    {
        /// <summary>
        /// Y level where mines will be generated.<br/>1 = Sea level.<br/>0 = Mantle.<br/>0.48(default) = In a regular world is Y-54.
        /// </summary>
        public double yLevel;

        /// <summary>
        /// Generate or not aditional ores in the mines.<br/>true = Generate aditionar ores.<br/>false = Don't generate aditional ores.
        /// </summary>
        public bool aditionalOres;

        public Config()
        {
        }

        public Config(double yLevel, bool aditionalOres)
        {
            this.yLevel = yLevel;
            this.aditionalOres = aditionalOres;
        }

    }
}