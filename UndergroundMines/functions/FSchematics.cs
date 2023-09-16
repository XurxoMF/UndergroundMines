using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace UndergroundMines
{
    public class FSchematics
    {
        public static Dictionary<EnumSchematicsType, List<BlockSchematic>> LoadSchematics(ICoreServerAPI api)
        {
            Dictionary<EnumSchematicsType, List<BlockSchematic>> schematics = new ();

            // underground/circle/
            var undergroundCircle = LoadSchematicsWithRotationsByPath(api, "underground/circle/");
            if (undergroundCircle != null) schematics.Add(EnumSchematicsType.UndergroundCircle, undergroundCircle);

            // underground/cross/
            var undergroundCross = LoadSchematicsWithRotationsByPath(api, "underground/cross/");
            if (undergroundCross != null) schematics.Add(EnumSchematicsType.UndergroundCross, undergroundCross);

            // underground/end/
            var undergroundEnd = LoadSchematicsWithRotationsByPath(api, "underground/end/");
            if (undergroundEnd != null) schematics.Add(EnumSchematicsType.UndergroundEnd, undergroundEnd);

            // underground/mine/
            var undergroundMine = LoadSchematicsWithRotationsByPath(api, "underground/mine/");
            if (undergroundMine != null) schematics.Add(EnumSchematicsType.UndergroundMine, undergroundMine);

            return schematics;
        }

        /// <param name="path">Starts in <c>undergroundmines:worldgen/schematics/</c><br/>
        /// Example: <c>overground/entrance/</c></param>
        private static List<BlockSchematic> LoadSchematicsWithRotationsByPath(ICoreServerAPI api, string path)
        {
            List<BlockSchematic> schematics = new ();

            List<IAsset> assets = api.Assets.GetManyInCategory("worldgen", $"schematics/{path}", "undergroundmines");

            foreach (IAsset asset in assets)
            {
                string fileName = asset.Name[..^5];

                BlockSchematic schematic = asset.ToObject<BlockSchematic>();

                if (schematic != null)
                {
                    schematics.Add(schematic);
                    api.Server.LogEvent($"[{ModInfo.MOD_NAME}] Loaded structure {fileName}");
                } else {
                    api.Server.LogError($"[{ModInfo.MOD_NAME}] Could not load the structure {fileName}");
                }
            }

            return schematics.Count > 0 ? schematics : null;
        }

        public static void Place(IBlockAccessor blockAccessor, IWorldAccessor world, Chunk chunk, BlockSchematic schematic, EnumRotation rotation)
        {
            schematic.ClonePacked();
            schematic.TransformWhilePacked(world, EnumOrigin.BottomCenter, (int)rotation);
            schematic.Init(blockAccessor);
            BlockPos pos = new (
                chunk.BlockX, chunk.BlockY, chunk.BlockZ
            );
            schematic.Place(blockAccessor, world, schematic.GetStartPos(pos, EnumOrigin.BottomCenter));
        }
    }
}