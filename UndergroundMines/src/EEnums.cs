namespace UndergroundMines
{
    public class ModInfo
    {
        public const string MOD_NAME = "UndergroundMines";
    }

    public enum EnumSchematicsType
    {
        undergroundCircle,
        undergroundCross,
        undergroundEnd,
        undergroundMine
    }

    public enum EnumRotation: int
    {
        north = 0,
        east = 90,
        south = 180,
        west = 270
    }
}