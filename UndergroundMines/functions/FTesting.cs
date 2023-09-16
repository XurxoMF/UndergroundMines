using System;

namespace UndergroundMines
{
    public class FTesting
    {
        /// <summary>
        /// For testing purposes only!
        /// </summary>
        /// <returns></returns>
        public static EnumRotation GetRandomRotation()
        {
            int rand = new Random().Next(4);
            var rotation = rand switch
            {
                0 => EnumRotation.North,
                1 => EnumRotation.East,
                2 => EnumRotation.South,
                3 => EnumRotation.West,
                _ => EnumRotation.North,
            };
            return rotation;
        }

        public static int CoordinateByChunk(int chunk, int worldLength, int chunkLength)
        {
            return (worldLength / 2) - chunk + (chunkLength / 2);
        }
    }
}