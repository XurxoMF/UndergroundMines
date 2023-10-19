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
            var undergroundCross = LoadSchematicsByPath(api, "underground/cross/");
            if (undergroundCross != null) schematics.Add(ESchematicType.UndergroundCross, undergroundCross);

            // underground/end/
            var undergroundEnd = LoadSchematicsByPath(api, "underground/end/");
            if (undergroundEnd != null) schematics.Add(ESchematicType.UndergroundEnd, undergroundEnd);

            // underground/mine/
            var undergroundMine = LoadSchematicsByPath(api, "underground/mine/");
            if (undergroundMine != null) schematics.Add(ESchematicType.UndergroundMine, undergroundMine);

            // underground/angle/
            var undergroundAngle = LoadSchematicsByPath(api, "underground/angle/");
            if (undergroundAngle != null) schematics.Add(ESchematicType.UndergroundAngle, undergroundAngle);

            // entrance/cross/
            var entranceCross = LoadSchematicsByPath(api, "entrance/cross/");
            if (entranceCross != null) schematics.Add(ESchematicType.EntranceCross, entranceCross);

            // entrance/entrance/
            var entranceEntrance = LoadSchematicsByPath(api, "entrance/entrance/");
            if (entranceEntrance != null) schematics.Add(ESchematicType.EntranceEntrance, entranceEntrance);

            // entrance/shaft/
            var entranceShaft = LoadSchematicsByPath(api, "entrance/shaft/");
            if (entranceShaft != null) schematics.Add(ESchematicType.EntranceShaft, entranceShaft);

            return schematics;
        }

        /// <param name="path">Starts in <c>undergroundmines:worldgen/schematics/</c><br/>
        /// Example: <c>overground/entrance/</c></param>
        private static List<BlockSchematic> LoadSchematicsByPath(ICoreServerAPI api, string path)
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

        /// <summary>
        /// Will place the structure replcing all and changing ores and rock type to match the terrain.
        /// </summary>
        /// <param name="config">Mod config.</param>
        /// <param name="blockAccessor">Block accesor./param>
        /// <param name="world">World accessor.</param>
        /// <param name="chunk">Chunk where we want to place the structure.</param>
        /// <param name="schematic">Schematic we want to place.</param>
        /// <param name="rotation">Rotation of the schematic.</param>
        /// <param name="rockType">Rock type in the are to replace ores and rocks.</param>
        /// <param name="api">Server API.</param>
        public static void PlaceReplacingAll(Config config, IBlockAccessor blockAccessor, IWorldAccessor world, Chunk chunk, BlockSchematic schematic, ERotation rotation, string rockType, ICoreServerAPI api)
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

        /// <summary>
        /// Will place the structure without replacing anything. Always facing North.
        /// </summary>
        /// <param name="blockAccessor">Block accesor./param>
        /// <param name="world">World accessor.</param>
        /// <param name="chunk">Chunk where we want to place the structure.</param>
        /// <param name="schematic">Schematic we want to place.</param>
        public static void PlaceWithoutReplacing(IBlockAccessor blockAccessor, IWorldAccessor world, Chunk chunk, BlockSchematic schematic)
        {
            var newSchematic = schematic.ClonePacked();

            // Entrances will always be in North rotation so we don't have to rotate it.
            newSchematic.Init(blockAccessor);
            BlockPos pos = new(
                chunk.BlockX, chunk.BlockY, chunk.BlockZ
            );
            newSchematic.Place(blockAccessor, world, newSchematic.GetStartPos(pos, EnumOrigin.BottomCenter), EnumReplaceMode.ReplaceAllNoAir);
        }
    }
}