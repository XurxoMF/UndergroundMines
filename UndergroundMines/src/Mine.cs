using ProtoBuf;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace UndergroundMines
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Mine
    {
        public EnumSchematicsType Type;
        public EnumRotation Rotation;
        public BlockSchematic Schematic;

        public Mine(EnumSchematicsType type, EnumRotation rotation, BlockSchematic schematic)
        {
            Type = type;
            Rotation = rotation;
            Schematic = schematic;
        }

        public void Place(IBlockAccessor _blockAccessor, IWorldAccessor _world, ChunkPos chunkPos)
        {
            BlockPos pos = new BlockPos(
                chunkPos.BlockX, chunkPos.BlockY, chunkPos.BlockZ
            );
            Schematic.Place(_blockAccessor, _world, Schematic.GetStartPos(pos, EnumOrigin.BottomCenter));
        }
    }
}