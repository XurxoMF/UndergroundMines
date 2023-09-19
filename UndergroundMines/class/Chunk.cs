using ProtoBuf;

namespace UndergroundMines
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Chunk
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
        public Chunk(int blockX, int blockY, int blockZ, int chunkSize)
        {
            X = blockX / chunkSize;
            Z = blockZ / chunkSize;
            BlockX = blockX;
            BlockY = blockY;
            BlockZ = blockZ;
        }

        public Chunk()
        {
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Chunk other = (Chunk)obj;
            return X == other.X && BlockY == other.BlockY && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return (X * 397) ^ Z;
        }
    }
}