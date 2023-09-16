using System;
using System.Collections.Generic;
using System.IO;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace UndergroundMines
{
    public class UndergroundMinesModSystem : ModSystem
    {
        private ICoreServerAPI _api;

        private int _chunkSize;

        private int _seaLevel;

        private WorldDimensions _wd;

        private Dictionary<EnumSchematicsType, List<BlockSchematic>> _schematics;

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
            _wd = new WorldDimensions(_api.WorldManager.MapSizeX, _api.WorldManager.MapSizeY, _api.WorldManager.MapSizeZ);

            _schematics = FSchematics.LoadSchematics(_api);

            _api.Event.ChunkColumnGeneration(OnChunkColumnGeneration, EnumWorldGenPass.Vegetation, "standard");
            _api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);

            _api.Event.GameWorldSave += SaveData;
            _savedData = LoadData();

            _api.Server.LogEvent($"[{ModInfo.MOD_NAME}] Ready!");
        }

        private void OnChunkColumnGeneration(IChunkColumnGenerateRequest request)
        {
            Chunk chunk = new (request.ChunkX * _chunkSize + (_chunkSize / 2), (int)Math.Round(_seaLevel * 0.66), request.ChunkZ * _chunkSize + (_chunkSize / 2));
            
            if (_savedData.GeneratedMines.Count < 30) {
                Mine mine = new (
                    EnumSchematicsType.UndergroundCross,
                    EnumRotation.North
                );

                FSchematics.Place(_blockAccessor, _world, chunk, _schematics[EnumSchematicsType.UndergroundCross][0], FTesting.GetRandomRotation());

                _api.Server.LogDebug($"[{ModInfo.MOD_NAME}] Estructura colocada en X {FTesting.CoordinateByChunk(chunk.X, _wd.X, _chunkSize)} | Y {chunk.BlockY} | Z {FTesting.CoordinateByChunk(chunk.X, _wd.X, _chunkSize)}\n> World {_wd.X}x{_wd.Z} | ChunkSize {_chunkSize}");

                _savedData.GeneratedMines.Add(chunk, mine);
                _savedData.Modified = true;
            }
        }

        private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
        {
            _blockAccessor = chunkProvider.GetBlockAccessor(false);
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