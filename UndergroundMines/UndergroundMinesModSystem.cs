using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace UndergroundMines
{
    public class UndergroundMinesModSystem : ModSystem
    {
        private ICoreServerAPI _api;
        private int _chunkSize;
        private int _seaLevel;
        private WorldDimensions _worldDimensions;
        private Dictionary<EnumSchematicsType, List<Dictionary<EnumRotation, BlockSchematic>>> _schematics;
        /** above _schematics explanation ^^
        [
            {SchematicsType.overgroundEntrance, [
                [
                    {Rotation.north, BlockSchematic},
                    {Rotation.east, BlockSchematic},
                    {Rotation.south, BlockSchematic},
                    {Rotation.west, BlockSchematic},
                ],
                [
                    {Rotation.north, BlockSchematic},
                    {Rotation.east, BlockSchematic},
                    {Rotation.south, BlockSchematic},
                    {Rotation.west, BlockSchematic},
                ],
                ...
            ]},
            {SchematicsType.undergroundCircle, [
                [
                    {Rotation.north, BlockSchematic},
                    {Rotation.east, BlockSchematic},
                    {Rotation.south, BlockSchematic},
                    {Rotation.west, BlockSchematic},
                ],
                [
                    {Rotation.north, BlockSchematic},
                    {Rotation.east, BlockSchematic},
                    {Rotation.south, BlockSchematic},
                    {Rotation.west, BlockSchematic},
                ],
                ...
            ]},
            ...
        ]
        */
        private IWorldGenBlockAccessor _blockAccessor;
        private IWorldAccessor _world;
        private SavedData _savedData;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            base.StartServerSide(api);

            _api = api;
            _world = _api.World;
            _seaLevel = _api.World.SeaLevel;
            _chunkSize = _api.WorldManager.ChunkSize;
            _worldDimensions = new WorldDimensions(_api.WorldManager.MapSizeX, _api.WorldManager.MapSizeY, _api.WorldManager.MapSizeZ);

            LoadSchematics();

            _api.Event.ChunkColumnGeneration(OnChunkColumnGeneration, EnumWorldGenPass.Vegetation, "standard");
            _api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);

            _api.Event.GameWorldSave += SaveData;
            _savedData = LoadData();

            _api.Server.LogEvent($"[{ModInfo.MOD_NAME}] Ready!");
        }

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

        private void LoadSchematics()
        {
            _schematics = new ();

            // underground/circle/
            var undergroundCircle = LoadSchematicsWithRotationsByPath("underground/circle/");
            if (undergroundCircle != null) _schematics.Add(EnumSchematicsType.undergroundCircle, undergroundCircle);

            // underground/cross/
            var undergroundCross = LoadSchematicsWithRotationsByPath("underground/cross/");
            if (undergroundCross != null) _schematics.Add(EnumSchematicsType.undergroundCross, undergroundCross);

            // underground/end/
            var undergroundEnd = LoadSchematicsWithRotationsByPath("underground/end/");
            if (undergroundEnd != null) _schematics.Add(EnumSchematicsType.undergroundEnd, undergroundEnd);

            // underground/mine/
            var undergroundMine = LoadSchematicsWithRotationsByPath("underground/mine/");
            if (undergroundMine != null) _schematics.Add(EnumSchematicsType.undergroundMine, undergroundMine);
        }

        ///
        /// <param name="path">Starts in <c>undergroundmines:worldgen/schematics/</c><br/>
        /// Example: <c>overground/entrance/</c></param>
        private List<Dictionary<EnumRotation, BlockSchematic>> LoadSchematicsWithRotationsByPath(string path)
        {
            List<Dictionary<EnumRotation, BlockSchematic>> schematics = new ();

            List<IAsset> assets = _api.Assets.GetManyInCategory("worldgen", $"schematics/{path}", "undergroundmines");

            foreach (IAsset asset in assets)
            {
                string fileName = asset.Name[..^5];

                BlockSchematic schematic = asset.ToObject<BlockSchematic>();

                if (schematic != null)
                {
                    Dictionary<EnumRotation, BlockSchematic> rotations = new ();
                    foreach (EnumRotation rot in Enum.GetValues(typeof(EnumRotation)))
                    {
                        rotations.Add(rot, schematic.ClonePacked());
                        rotations[rot].TransformWhilePacked(_world, EnumOrigin.BottomCenter, (int)rot);
                    }

                    schematics.Add(rotations);

                    _api.Server.LogEvent($"[{ModInfo.MOD_NAME}] Loaded structure {fileName}");
                } else {
                    _api.Server.LogError($"[{ModInfo.MOD_NAME}] Could not load the structure {fileName}");
                }
            }

            return schematics.Count > 0 ? schematics : null;
        }

        private void OnChunkColumnGeneration(IChunkColumnGenerateRequest request)
        {
            ChunkPos chunk = new (request.ChunkX * _chunkSize + (_chunkSize / 2), 64, request.ChunkZ * _chunkSize + (_chunkSize / 2));
            
            if (_savedData.GeneratedMines.Count < 1) {
                _api.Server.LogDebug($"[{ModInfo.MOD_NAME}] EDentro de Generated.Count");

                Mine mine = new (
                    EnumSchematicsType.undergroundCross,
                    EnumRotation.north,
                    _schematics[EnumSchematicsType.undergroundCross][0][EnumRotation.north]
                );

                mine.Place(_blockAccessor, _world, chunk);

                _api.Server.LogDebug($"[{ModInfo.MOD_NAME}] Estrctura colocada en X {chunk.BlockX} | Y {chunk.BlockY} | Z {chunk.BlockZ}");

                _savedData.GeneratedMines.Add(chunk, mine);
                _savedData.Modified = true;
            }
        }

        private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
        {
            _blockAccessor = chunkProvider.GetBlockAccessor(false);
        }
    }
}