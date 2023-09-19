using System;
using Vintagestory.API.Server;

namespace UndergroundMines
{
    public class FTesting
    {
        /// <summary>
        /// For testing purposes only!
        /// </summary>
        /// <returns></returns>
        public static ERotation GetRandomRotation()
        {
            int rand = new Random().Next(4);
            var rotation = rand switch
            {
                0 => ERotation.North,
                1 => ERotation.East,
                2 => ERotation.South,
                3 => ERotation.West,
                _ => ERotation.North,
            };
            return rotation;
        }

        public static int CoordinateByChunk(int chunk, int worldLength, int chunkLength)
        {
            return chunk - (worldLength / 2);
        }

        public static void LogNewStructure(ICoreServerAPI api, Chunk chunk, WorldDimensions wd, int chunkSize)
        {
            api.Server.LogDebug($"[{ModInfo.MOD_NAME}] Estructura colocada en X {CoordinateByChunk(chunk.BlockX, wd.X, chunkSize)} | Y {chunk.BlockY} | Z {CoordinateByChunk(chunk.BlockZ, wd.Z, chunkSize)}\n> World {wd.X}x{wd.Z} | ChunkSize {chunkSize}");
        }
    }
}