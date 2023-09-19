using System.Collections.Generic;

namespace UndergroundMines
{
    public class ModInfo
    {
        public const string MOD_NAME = "UndergroundMines";
    }

    public enum ESchematicType
    {
        Null, // No structure, used to not generate anything
        UndergroundCross, // default exit all sides
        UndergroundEnd, // default exit only north
        UndergroundMine, // default exit north-south
        UndergroundAngle // default exit north-east
    }

    public enum ERotation: int
    {
        North = 0,
        East = 90,
        South = 180,
        West = 270
    }
}