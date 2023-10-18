using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace UndergroundMines
{
    public class UndergroundMinesModSystem : ModSystem
    {
        private ICoreServerAPI _api;

        private int _chunkSize;

        private int _seaLevel;

        private Dictionary<ESchematicType, List<BlockSchematic>> _schematics;

        private IWorldGenBlockAccessor _blockAccessor;

        private IWorldAccessor _world;

        private SavedData _savedData;

        private Config _config;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        public override double ExecuteOrder()
        {
            return 0.25;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            _api = api;
            _world = _api.World;
            _seaLevel = _api.World.SeaLevel;
            _chunkSize = _api.WorldManager.ChunkSize;

            _schematics = FSchematics.LoadSchematics(_api);

            _api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
            _api.Event.InitWorldGenerator(InitWorldGen, "standard");

            _api.Event.ChunkColumnGeneration(OnChunkColumnGeneration, EnumWorldGenPass.TerrainFeatures, "standard");

            _api.Event.GameWorldSave += SaveData;
            _savedData = LoadData();

            _api.Server.LogEvent($"[{ModInfo.MOD_NAME}] Ready!");
        }

        private void OnChunkColumnGeneration(IChunkColumnGenerateRequest request)
        {
            Chunk chunk = FChunk.GetChunk(_config, request.ChunkX, request.ChunkZ, _chunkSize, _seaLevel);
            Structure structure = null;
            bool generated = false;

            // Sides with exit or with no generated chunks NO SIDES WITHOUT STRUCTURES IN GENERATED CHUNKS
            var exits = FAlgorithms.CheckExitSides(_config, request.ChunkX, request.ChunkZ, _chunkSize, _seaLevel, _savedData, 1, _blockAccessor);

            if (exits.Count <= 0)
            { // No exit in any side
                structure = null;
                generated = true;
            }
            else
            { // Exit in some of the sides
                // Sides with exit and structure, NO SIDES WITHOUT STRUCTURE and NO SIDES WITHOUT CHUNK GENERATED.
                List<ERotation> structuredExits = FAlgorithms.CheckStructuredSides(_config, request.ChunkX, request.ChunkZ, _chunkSize, _seaLevel, exits, _savedData);
                // ONLY NOT GENERATED CHUNK SIDES
                List<ERotation> notGeneratedChunkExits = exits.Except(structuredExits).ToList();

                if (notGeneratedChunkExits.Count == 4)
                {
                    structure = new Structure(ESchematicType.UndergroundMine, ERotation.North);
                    generated = true;
                }
                else if (structuredExits.Count <= 0)
                { // No structure with direct exit in colindant chunks.
                    // 100% INFINITE
                    /*
                    if (exits.Count == 2)
                    {
                        if (!FAlgorithms.AreSidesOpposite(exits))
                        { // exits in angle
                            structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundAngle, exits);
                            generated = true;
                        }
                    }
                    else
                    {
                        var randomType = FAlgorithms.REndOrNull();

                        if (randomType == ESchematicType.Null)
                        {
                            structure = null;
                            generated = true;
                        }
                        else
                        {
                            structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, exits);
                            generated = true;
                        }
                    }
                    */
                    // 100% INFINITE

                    // NOT 100% INFINITE
                    var randomType = FAlgorithms.REndOrNull();

                    if (randomType == ESchematicType.Null)
                    {
                        structure = null;
                        generated = true;
                    }
                    else
                    {
                        structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, exits);
                        generated = true;
                    }
                    // NOT 100% INFINITE
                }
                else
                { // At least 1 structure with exit in colindant chunks.
                    if (structuredExits.Count == 1)
                    { // 1 side with structured exit
                        if (exits.Count == 1)
                        { // Only 1 exit and that one has structure.
                            structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundEnd, exits);
                            generated = true;
                        }
                        else if (exits.Count == 2)
                        { // 1 more exit not generated
                            if (FAlgorithms.AreSidesOpposite(exits))
                            { // exits are in opposite sides
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundMine, exits);
                                generated = true;
                            }
                            else
                            { // exits are in angle
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundAngle, exits);
                                generated = true;
                            }
                        }
                        else if (exits.Count == 3)
                        { // 2 more exits not generated
                            if (exits.Contains(FAlgorithms.GetOppositeSide(structuredExits[0])))
                            { // Exit in opposite side, can place a mine.
                                var randomType = FAlgorithms.RAngleOrMine();

                                if (randomType == ESchematicType.UndergroundAngle)
                                {
                                    var sides = FAlgorithms.GetRandomAngleSideFromList(structuredExits[0], exits);
                                    structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, sides);
                                    generated = true;
                                }
                                else
                                { // randomType == ESchematicType.UndergroundMine
                                    structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, new List<ERotation>() { FAlgorithms.GetOppositeSide(structuredExits[0]), structuredExits[0] });
                                    generated = true;
                                }
                            }
                            else
                            { // Only an angle can be placed
                                var sides = FAlgorithms.GetRandomAngleSideFromList(structuredExits[0], exits);
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundAngle, sides);
                                generated = true;
                            }
                        }
                        else
                        { // 3 more exits not generated
                            var randomType = FAlgorithms.RAngleOrMineOrCross();

                            if (randomType == ESchematicType.UndergroundCross)
                            {
                                structure = new Structure(randomType, ERotation.North);
                                generated = true;
                            }
                            else if (randomType == ESchematicType.UndergroundMine)
                            {
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, new List<ERotation>() { structuredExits[0], FAlgorithms.GetOppositeSide(structuredExits[0]) });
                                generated = true;
                            }
                            else
                            { // randomType == ESchematicType.UndergroundAngle
                                var sides = FAlgorithms.GetRandomAngleSideFromList(structuredExits[0], exits);
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, sides);
                                generated = true;
                            }
                        }
                    }
                    else if (structuredExits.Count == 2)
                    { // 2 sides with structured exit
                        if (exits.Count == 4)
                        { // 2 more sides not generated
                            if (FAlgorithms.AreSidesOpposite(structuredExits))
                            { // If sides are in opposite sides.
                                // We can't generate an angle here, one of the sides would end in nothing
                                var randomType = FAlgorithms.RCrossOrMine();

                                if (randomType == ESchematicType.UndergroundCross)
                                {
                                    structure = new Structure(randomType, ERotation.North);
                                    generated = true;
                                }
                                else
                                { // randomType == ESchematicType.UndergroundMine
                                    structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, structuredExits);
                                    generated = true;
                                }
                            }
                            else
                            { // If sides are in angle.
                                var randomType = FAlgorithms.RCrossOrAngle();

                                if (randomType == ESchematicType.UndergroundCross)
                                {
                                    structure = new Structure(randomType, ERotation.North);
                                    generated = true;
                                }
                                else
                                { // randomType == ESchematicType.UndergroundAngle
                                    structure = FAlgorithms.GetStructureWithAdjustedRotation(randomType, structuredExits);
                                    generated = true;
                                }
                            }
                        }
                        else if (exits.Count == 3)
                        { // 1 more side not generated
                            // For now on, I'll generate an angle or a mine depending on where structured sides are, but in the future a new T structure could appear.
                            if (FAlgorithms.AreSidesOpposite(structuredExits))
                            { // If sides are in opposite sides.
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundMine, structuredExits);
                                generated = true;
                            }
                            else
                            { // If sides are in angle.
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundAngle, structuredExits);
                                generated = true;
                            }
                        }
                        else
                        { // Only the 2 structured sides
                            if (FAlgorithms.AreSidesOpposite(structuredExits))
                            { // If sides are in opposite sides.
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundMine, structuredExits);
                                generated = true;
                            }
                            else
                            { // If sides are in angle.
                                structure = FAlgorithms.GetStructureWithAdjustedRotation(ESchematicType.UndergroundAngle, structuredExits);
                                generated = true;
                            }
                        }
                    }
                    else if (structuredExits.Count == 3)
                    { // 3 sides with structured exit
                        if (exits.Count == 4)
                        { // 1 more side not generated
                            // For now on I can only place a 4 side exit, it's the only one connecting 3 structures at once.
                            structure = new Structure(ESchematicType.UndergroundCross, ERotation.North);
                            generated = true;
                        }
                        else
                        { // Only the 3 structured sides
                            // ! I'm fucked! with any 3 exits structure I'll have to place a 4 side exits in a 3 sides exit, for now this will be a "bug".
                            structure = new Structure(ESchematicType.UndergroundCross, ERotation.North);
                            generated = true;
                        }
                    }
                    else if (structuredExits.Count == 4)
                    { // 4 sides with structured exit
                        structure = new Structure(ESchematicType.UndergroundCross, ERotation.North);
                        generated = true;
                    }
                }
            }

            // Placing the structure
            if (generated)
            {
                if (structure != null)
                {
                    // Gets a random schematic of the schematic type
                    BlockSchematic schematic = FSchematics.GetRandomSchematicByType(structure, _schematics);

                    // Data to look for the rock type in the area & rockType in the area
                    var chunkData = request.Chunks[chunk.BlockY / _chunkSize];
                    string rockType = FChunk.GetRockType(_chunkSize, chunkData, _world, chunk.BlockY % _chunkSize);

                    // Places the above schematic
                    FSchematics.Place(_config, _blockAccessor, _world, chunk, schematic, structure.Rotation, rockType, _api);
                }

                // Save the structure and chunk where it's placed
                try
                {
                    _savedData.GeneratedStructures.Add(chunk, structure);
                    _savedData.Modified = true;
                }
                catch (Exception err)
                {
                    _api.Server.LogError($"{err}");
                }
            }
        }

        private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
        {
            _blockAccessor = chunkProvider.GetBlockAccessor(false);
        }

        private void InitWorldGen()
        {
            // Try loading config.
            try
            {
                _config = _api.LoadModConfig<Config>($"{ModInfo.MOD_NAME}.json");
            }
            catch
            {
                _api.Logger.Error($"[{ModInfo.MOD_NAME}] [Config] There is no config file or it contain errors! Applying default config!");
                _config = new Config(ModStaticConfig.DefaultHeight, true);
            }

            // Value testing, in case some of them is invalid.
            if (_config.yLevel > 1 || _config.yLevel < 0)
            {
                _api.Logger.Error($"[{ModInfo.MOD_NAME}] [Config] yLevel has to be a number between 0 and 1! Changing to default ({ModStaticConfig.DefaultHeight})!");
                _config.yLevel = ModStaticConfig.DefaultHeight;
            }

            _api.StoreModConfig(_config, $"{ModInfo.MOD_NAME}.json");
            _api.Logger.Event($"[{ModInfo.MOD_NAME}] [Config] Config succesfully loaded.");
        }

        // DATA MANAGING
        private void SaveData()
        {
            if (!_savedData.Modified) return;

            var dungeonDataFile = Path.Combine(GamePaths.DataPath, "ModData", _api.WorldManager.SaveGame.SavegameIdentifier, $"{ModInfo.MOD_NAME}.bin");

            var data = SerializerUtil.Serialize(_savedData);

            File.WriteAllBytes(dungeonDataFile, data);

            _savedData.Modified = false;
        }

        private SavedData LoadData()
        {
            var dataFolder = Path.Combine(GamePaths.DataPath, "ModData", _api.WorldManager.SaveGame.SavegameIdentifier);

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
                return new SavedData();
            }
            else
            {
                var dataFile = Path.Combine(dataFolder, $"{ModInfo.MOD_NAME}.bin");
                return File.Exists(dataFile)
                    ? SerializerUtil.Deserialize<SavedData>(File.ReadAllBytes(dataFile))
                    : new SavedData();
            }
        }

    }
}