using System;

namespace UndergroundMines
{
    public class FChunk
    {
        public static Chunk GetChunk(int chunkX, int chunkZ, int chunkSize, int seaLevel)
        {
            return new (chunkX * chunkSize + (chunkSize / 2), (int)Math.Round(seaLevel * 0.66), chunkZ * chunkSize + (chunkSize / 2));
        }
    }
}