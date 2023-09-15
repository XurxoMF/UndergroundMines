namespace UndergroundMines
{
    public enum SchematicsType
    {
        horizontalSchematics,
        verticalSchematics,
        angledLeftUpSchematics,
        angledLeftDownSchematics,
        angledRightUpSchematics,
        angledRightDownSchematics,
        simpleShafts,
        deadEnd,
        Empty,
        descentUpper,
        descentLower,
        descentMiddle,
    }

    public enum Angle
    {
        north = 0,
        east = 90,
        south = 180,
        west = 270,
    }

}