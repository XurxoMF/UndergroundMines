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

        public static void Place(Config config, IBlockAccessor blockAccessor, IWorldAccessor world, Chunk chunk, BlockSchematic schematic, ERotation rotation, string rockType, ICoreServerAPI api)
        {
            var newSchematic = schematic.ClonePacked();

            // Replace {rocktype}, {ore1} & {ore2} with the rockType
            var keys = newSchematic.BlockCodes.Keys;

            Dictionary<int, AssetLocation> BlockCodes = new();

            var ores = ModStaticConfig.RockTypeAndOres[rockType];

            string ore1 = null;
            string ore2 = null;

            if (ores != null)
            {
                ore1 = ores[new Random().Next(ores.Length)];
                ore2 = ores[new Random().Next(ores.Length)];
            }

            foreach (var key in keys)
            {
                var asset = "game:" + schematic.BlockCodes[key].Path;

                if (asset.Contains("{rocktype}"))
                {
                    asset = asset.Replace("{rocktype}", rockType);
                }
                else if (asset.Contains("{orerock}"))
                {
                    if (config.aditionalOres)
                    {
                        if (ores != null) // If there is any ore with that rock type
                        {
                            asset = asset.Replace("{ore1}", ore1).Replace("{ore2}", ore2).Replace("{orerock}", rockType);
                        }
                        else // If there is no ores with that rock type
                        {
                            asset = asset.Replace("{ore1}", "poor-nativecopper").Replace("{ore2}", "quartz").Replace("{orerock}", "andesite");
                        }
                    }
                    else
                    {
                        asset = "null";
                    }
                }

                AssetLocation newBlock = new(asset);
                BlockCodes.Add(key, newBlock);
            }
            newSchematic.BlockCodes = BlockCodes;

            // Rotate the structure, Init and Place
            newSchematic.TransformWhilePacked(world, EnumOrigin.BottomCenter, (int)rotation);
            newSchematic.Init(blockAccessor);
            BlockPos pos = new(
                chunk.BlockX, chunk.BlockY, chunk.BlockZ
            );
            newSchematic.Place(blockAccessor, world, newSchematic.GetStartPos(pos, EnumOrigin.BottomCenter), EnumReplaceMode.ReplaceAllNoAir);
        }
    }
}