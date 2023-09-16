using System;
using ProtoBuf;
using Vintagestory.API.MathTools;

namespace UndergroundMines
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ChunkPos
    {
        /// <summary>
        /// Chunk position in X
        /// </summary>
        public int X;

        /// <summary>
        /// Chunk position in Z
        /// </summary>
        public int Z;

        /// <summary>
        /// Block position in X to place structure
        /// </summary>
        public int BlockX;

        /// <summary>
        /// Block position in Y to place structure
        /// </summary>
        public int BlockY;

        /// <summary>
        /// Block position in Z to place structure
        /// </summary>
        public int BlockZ;

        /// <param name="blockX">Block position in X to place structure</param>
        /// <param name="blockY">Block position in Y to place structure</param>
        /// <param name="blockZ">Block position in Z to place structure</param>
        public ChunkPos(int blockX, int blockY, int blockZ)
        {
            X = blockX / 32;
            Z = blockY / 32;
            BlockX = blockX;
            BlockY = blockY;
            BlockZ = blockZ;
        }
    }
}