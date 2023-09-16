namespace UndergroundMines
{
    public class ModInfo
    {
        public const string MOD_NAME = "UndergroundMines";
    }

    public enum EnumSchematicsType
    {
        UndergroundCircle,
        UndergroundCross,
        UndergroundEnd,
        UndergroundMine
    }

    public enum EnumRotation: int
    {
        North = 0,
        East = 90,
        South = 180,
        West = 270
    }
}