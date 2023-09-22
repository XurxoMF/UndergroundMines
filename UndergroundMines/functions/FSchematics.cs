using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace UndergroundMines
{
    public class FSchematics
    {
        public static Dictionary<ESchematicType, List<BlockSchematic>> LoadSchematics(ICoreServerAPI api)
        {
            Dictionary<ESchematicType, List<BlockSchematic>> schematics = new();

            // underground/cross/
            var undergroundCross = LoadSchematicsWithRotationsByPath(api, "underground/cross/");
            if (undergroundCross != null) schematics.Add(ESchematicType.UndergroundCross, undergroundCross);

            // underground/end/
            var undergroundEnd = LoadSchematicsWithRotationsByPath(api, "underground/end/");
            if (undergroundEnd != null) schematics.Add(ESchematicType.UndergroundEnd, undergroundEnd);

            // underground/mine/
            var undergroundMine = LoadSchematicsWithRotationsByPath(api, "underground/mine/");
            if (undergroundMine != null) schematics.Add(ESchematicType.UndergroundMine, undergroundMine);

            // underground/angle/
            var undergroundAngle = LoadSchematicsWithRotationsByPath(api, "underground/angle/");
            if (undergroundAngle != null) schematics.Add(ESchematicType.UndergroundAngle, undergroundAngle);

            return schematics;
        }

        /// <param name="path">Starts in <c>undergroundmines:worldgen/schematics/</c><br/>
        /// Example: <c>overground/entrance/</c></param>
        private static List<BlockSchematic> LoadSchematicsWithRotationsByPath(ICoreServerAPI api, string path)
        {
            List<BlockSchematic> schematics = new();

            List<IAsset> assets = api.Assets.GetManyInCategory("worldgen", $"schematics/{path}", "undergroundmines");

            foreach (IAsset asset in assets)
            {
                string fileName = asset.Name[..^5];

                BlockSchematic schematic = asset.ToObject<BlockSchematic>();

                if (schematic != null)
                {
                    schematics.Add(schematic);
                    api.Server.LogEvent($"[{ModInfo.MOD_NAME}] Loaded structure {fileName}");
                }
                else
                {
                    api.Server.LogError($"[{ModInfo.MOD_NAME}] Could not load the structure {fileName}");
                }
            }

            return schematics.Count > 0 ? schematics : null;
        }

        public static BlockSchematic GetRandomSchematicByType(Structure structure, Dictionary<ESchematicType, List<BlockSchematic>> schematics)
        {
            return schematics[structure.Type][new Random().Next(schematics[structure.Type].Count)];
        }

        public static void Place(IBlockAccessor blockAccessor, IWorldAccessor world, Chunk chunk, BlockSchematic schematic, ERotation rotation, string rockType)
        {
            var newSchematic = schematic.ClonePacked();

            // var keys = newSchematic.BlockCodes.Keys;
            // Dictionary<int, AssetLocation> BlockCodes = new();
            // foreach (var item in keys)
            // {
            //     AssetLocation newBlock = new(schematic.BlockCodes[item].Path.Replace("{rock}", rockType));
            //     BlockCodes.Add(item, newBlock);
            // }
            // schematic.BlockCodes = BlockCodes;

            newSchematic.TransformWhilePacked(world, EnumOrigin.BottomCenter, (int)rotation);
            newSchematic.Init(blockAccessor);
            BlockPos pos = new(
                chunk.BlockX, chunk.BlockY, chunk.BlockZ
            );
            newSchematic.Place(blockAccessor, world, newSchematic.GetStartPos(pos, EnumOrigin.BottomCenter));
        }
    }
}