using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace UndergroundMines
{
    public class FChunk
    {
        public static Chunk GetChunk(int chunkX, int chunkZ, int chunkSize, int seaLevel)
        {
            return new(chunkX * chunkSize + (chunkSize / 2), 150/*(int)Math.Round(seaLevel * 0.48)*/, chunkZ * chunkSize + (chunkSize / 2), chunkSize);
        }

        public static string GetRockType(int chunkSize, IServerChunk chunkData, IWorldAccessor world, int start)
        {
            string rockType = "";

            for (int y = start; y >= 0 && rockType == ""; y--)
            {
                var path = chunkData.GetLocalBlockAtBlockPos(world, new BlockPos(chunkSize / 2, y, chunkSize / 2)).Code.Path;

                if (path.StartsWith("rock-"))
                {
                    rockType = path.Split("-")[1];
                }
                else if (path.StartsWith("ore-"))
                {
                    rockType = path.Split("-").Last();
                }
                else if (path.StartsWith("crackedrock-"))
                {
                    rockType = path.Split("-")[1];
                }
            }

            if (rockType == "") rockType = "andesite";

            if (!ModConfig.RockTypeAndOres.ContainsKey(rockType)) rockType = "andesite";

            return rockType;
        }
    }
}