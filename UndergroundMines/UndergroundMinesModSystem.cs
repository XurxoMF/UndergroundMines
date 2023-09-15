using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace UndergroundMines
{
    public class UndergroundMinesModSystem : ModSystem
    {
        private ICoreServerAPI api;
        private int chunkSize;
        private int stairLevel;
        private Dictionary<SchematicsType, BlockSchematic[]> schematics;
        private BlockSchematic[] horizontalSchematics;
        private BlockSchematic[] verticalSchematics;
        private BlockSchematic[] crossSchematics;
        private BlockSchematic[] angleSchematics;
        private BlockSchematic[] counterAngleSchematics;
        private BlockSchematic[] emptySchematics;
        private BlockSchematic[] deadEndSchematics;
        private BlockSchematic[] descentUpper;
        private BlockSchematic[] descentLower;
        private BlockSchematic[] descentMiddle;
        private BlockPos schematicCenterPos;
        private int seaLevel;
        private int chunkCenterX;
        private int chunkCenterZ;
        private BlockPos chunkCenterPos;
        private IWorldGenBlockAccessor blockAccessor;
        private IWorldAccessor worldForResolve;
        private int stairMultiplier1stLevel;

        public override bool ShouldLoad(EnumAppSide forSide)
        {
            return forSide == EnumAppSide.Server;
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            this.api = api;
            worldForResolve = api.World;
            seaLevel = api.World.SeaLevel;
            chunkSize = api.WorldManager.ChunkSize;
            stairLevel = 5;
            LoadSchematics();
            stairMultiplier1stLevel = 7;
            this.api.Event.ChunkColumnGeneration(OnChunkColumnGeneration, EnumWorldGenPass.Vegetation, "standard");
            this.api.Event.GetWorldgenBlockAccessor(OnWorldGenBlockAccessor);
            api.Server.LogEvent("Underground Mines loaded!");
        }

        public void LoadSchematics()
        {
            BlockSchematic horizontalSchematic_1 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/line1.json")).ToObject<BlockSchematic>();
            BlockSchematic horizontalSchematic_2 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/line2.json")).ToObject<BlockSchematic>();
            BlockSchematic horizontalSchematic_3 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/line3.json")).ToObject<BlockSchematic>();
            BlockSchematic horizontalSchematic_4 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/line4.json")).ToObject<BlockSchematic>();
            BlockSchematic horizontalSchematic_5 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/line5.json")).ToObject<BlockSchematic>();
            BlockSchematic horizontalSchematic_6 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/line6.json")).ToObject<BlockSchematic>();
  
            BlockSchematic crossSchematic_1 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/cross1.json")).ToObject<BlockSchematic>();
            BlockSchematic crossSchematic_2 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/cross2.json")).ToObject<BlockSchematic>();

            BlockSchematic angleSchematic_1 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/angle1.json")).ToObject<BlockSchematic>();
            BlockSchematic angleSchematic_2 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/angle2.json")).ToObject<BlockSchematic>();
            BlockSchematic angleSchematic_3 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/angle3.json")).ToObject<BlockSchematic>();
            BlockSchematic angleSchematic_4 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/shafts/angle4.json")).ToObject<BlockSchematic>();

            BlockSchematic deadEnd_1 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/deadend/deadend1.json")).ToObject<BlockSchematic>();
            BlockSchematic deadEnd_2 = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/deadend/deadendore.json")).ToObject<BlockSchematic>();

            string[] shafts = { "mineshaft1", "mineshaft2", "mineshaft3", "mineshaft4", "mineshaft5", "mineshaft6" };
            var shaftsSchematics = new List<BlockSchematic>();
            foreach (string shaft in shafts)  {
                shaftsSchematics.Add(api.Assets.Get(
                            new AssetLocation("game", $"worldgen/schematics/shafts/{shaft}.json")).ToObject<BlockSchematic>());
            }

            BlockSchematic stemMiddle = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/descent/stemmiddle.json")).ToObject<BlockSchematic>();
            BlockSchematic stemDown = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/descent/stemdown.json")).ToObject<BlockSchematic>();
            BlockSchematic stemUp = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/descent/stemup.json")).ToObject<BlockSchematic>();

            BlockSchematic empty = api.Assets.Get(
                            new AssetLocation("game", "worldgen/schematics/overground/flu.json")).ToObject<BlockSchematic>();

            horizontalSchematics = new BlockSchematic[] {
                horizontalSchematic_1,
                horizontalSchematic_2, 
                horizontalSchematic_3,
                horizontalSchematic_4, 
                horizontalSchematic_5,
                horizontalSchematic_6
            };

            verticalSchematics = new BlockSchematic[] {
                horizontalSchematic_1,
                horizontalSchematic_2,
                horizontalSchematic_3,
                horizontalSchematic_4, 
                horizontalSchematic_5,
                horizontalSchematic_6
            };

            crossSchematics = new BlockSchematic[] {
                crossSchematic_1,
                crossSchematic_2
            };

            angleSchematics = new BlockSchematic[] {
                angleSchematic_1,
                angleSchematic_2,
                angleSchematic_3,
                angleSchematic_4
            };

            counterAngleSchematics = new BlockSchematic[] {
                angleSchematic_2,
                angleSchematic_3
            };

            emptySchematics = new BlockSchematic[] {
                empty
            };

            deadEndSchematics = new BlockSchematic[] {
                deadEnd_1,
                deadEnd_2
            };

            descentUpper = new BlockSchematic[] {
                stemUp
            };

            descentLower = new BlockSchematic[] {
                stemDown
            };

            descentMiddle = new BlockSchematic[] {
                stemMiddle
            };

            schematics = new Dictionary<SchematicsType, BlockSchematic[]>()
            {
                { SchematicsType.horizontalSchematics, horizontalSchematics },
                { SchematicsType.verticalSchematics, verticalSchematics},
                { SchematicsType.angledLeftUpSchematics, angleSchematics},
                { SchematicsType.angledLeftDownSchematics, crossSchematics},
                { SchematicsType.angledRightDownSchematics, counterAngleSchematics},
                { SchematicsType.simpleShafts, shaftsSchematics.ToArray()},
                { SchematicsType.Empty, emptySchematics},
                { SchematicsType.deadEnd, deadEndSchematics},
                { SchematicsType.descentLower, descentLower},
                { SchematicsType.descentUpper, descentUpper},
                { SchematicsType.descentMiddle, descentMiddle},
            };
        }

        private void ChunkCenter1stLevel(int chunkX, int chunkZ)
        {
            chunkCenterX = (chunkX * chunkSize) +  (chunkSize / 2);
            chunkCenterZ = (chunkZ * chunkSize) +  (chunkSize / 2);
            api.Server.LogVerboseDebug($"2 chunk center X {chunkCenterX} - Z {chunkCenterZ}");
            chunkCenterPos = new BlockPos
            {
                X = chunkCenterX,
                Y = seaLevel - Math.Abs( stairLevel * stairMultiplier1stLevel ),
                Z = chunkCenterZ
            };
        }

        private void CenterBlockpos()
        {
            schematicCenterPos = new BlockPos
            {
                X = 17,
                Y = 0,
                Z = 17
            };
        }

        private void OnChunkColumnGeneration(IChunkColumnGenerateRequest request)
        {
            int chunkX = request.ChunkX;
            int chunkZ = request.ChunkZ;

            ChunkCenter1stLevel(chunkX, chunkZ);
            CenterBlockpos();
            var rand = new Random();
            var typeOfSelectedSchematicsX = SelectSchematicTypeXAxis(chunkX, chunkZ);
            var typeOfSelectedSchematicsZ = SelectSchematicTypeZAxis(chunkX, chunkZ);
            var typeOfSelectedSchematicsSimple = SimpleShaftsType(chunkX, chunkZ);
            var typeOfSelectedSchematicsAdditional = SelectSchematicTypeZAxisAdditional(chunkX, chunkZ);

            if (typeOfSelectedSchematicsX != SchematicsType.Empty)
            {
                BlockSchematic[] chematicsTypeArrX = schematics[typeOfSelectedSchematicsX];
                var SelectedSchematicX = chematicsTypeArrX[rand.Next(chematicsTypeArrX.Length)];
                BlockPos diffCenterX = SelectedSchematicX.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                SelectedSchematicX.ClonePacked();
                SelectedSchematicX.Init(blockAccessor);
                SelectedSchematicX.Place(blockAccessor, api.World, diffCenterX, EnumReplaceMode.ReplaceAllNoAir, true);
                
            }

            if (typeOfSelectedSchematicsZ != SchematicsType.Empty)
            {
                BlockSchematic[] chematicsTypeArrZ = schematics[typeOfSelectedSchematicsZ];
                int randDeadEnd = rand.Next(0, 20);
                BlockSchematic SelectedSchematicZ;
                if (randDeadEnd == 15)
                {
                    SelectedSchematicZ = deadEndSchematics[rand.Next(0, deadEndSchematics.Length)];
                    SelectedSchematicZ.Init(blockAccessor);
                    SelectedSchematicZ.ClonePacked();
                    SelectedSchematicZ.TransformWhilePacked(worldForResolve, EnumOrigin.BottomCenter, 90);
                    BlockPos diffCenterZ = SelectedSchematicZ.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                    SelectedSchematicZ.Place(blockAccessor, api.World, diffCenterZ, EnumReplaceMode.ReplaceAllNoAir, true);

                }
                else
                {
                    SelectedSchematicZ = chematicsTypeArrZ[rand.Next(0, chematicsTypeArrZ.Length)];
                    SelectedSchematicZ.Init(blockAccessor);
                    SelectedSchematicZ.ClonePacked();
                    BlockPos diffCenterZ = SelectedSchematicZ.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                    SelectedSchematicZ.Place(blockAccessor, api.World, diffCenterZ, EnumReplaceMode.ReplaceAllNoAir, true);


                }
            }

            if (typeOfSelectedSchematicsSimple != SchematicsType.Empty)
            {
                BlockSchematic[] chematicsTypeArrSimple = schematics[typeOfSelectedSchematicsSimple];
                var SelectedSchematicSimple = chematicsTypeArrSimple[rand.Next(0, chematicsTypeArrSimple.Length)];
                SelectedSchematicSimple.Init(blockAccessor);
                SelectedSchematicSimple.TransformWhilePacked(worldForResolve, EnumOrigin.BottomCenter, (int)Angle.east);
                BlockPos diffCenterSimple = SelectedSchematicSimple.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                SelectedSchematicSimple.Place(blockAccessor, api.World, diffCenterSimple, EnumReplaceMode.ReplaceAllNoAir, true);
            }
            if (typeOfSelectedSchematicsAdditional != SchematicsType.Empty)
            {
                BlockSchematic[] chematicsTypeAdditional = schematics[typeOfSelectedSchematicsAdditional];
                var SelectedSchematicAdditional = chematicsTypeAdditional[rand.Next(0,chematicsTypeAdditional.Length)];
                SelectedSchematicAdditional.Init(blockAccessor);
                SelectedSchematicAdditional.TransformWhilePacked(worldForResolve, EnumOrigin.BottomCenter, 90);
                BlockPos diffCenterAdd = SelectedSchematicAdditional.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                SelectedSchematicAdditional.Place(blockAccessor, api.World, diffCenterAdd, EnumReplaceMode.ReplaceAllNoAir, true);
            }
            
            if ((typeOfSelectedSchematicsX != SchematicsType.Empty) & (typeOfSelectedSchematicsZ != SchematicsType.Empty))
            {
                int mapHeight = blockAccessor.GetTerrainMapheightAt(chunkCenterPos);
                if (mapHeight < (chunkCenterPos.Y + stairLevel * stairMultiplier1stLevel) & (rand.Next(0, 15) == 12))
                {
                    int descentCount = (mapHeight - chunkCenterPos.Y )/ stairLevel;
                    for (int i = 0; i < (descentCount + 1); i++)
                    {
                        if (i == 1)
                        {
                            BlockSchematic[] descentLower = schematics[SchematicsType.descentLower];
                            var SelectedSchematicDescentLower = descentLower[rand.Next(0, descentLower.Length)];
                            SelectedSchematicDescentLower.Init(blockAccessor);
                            var pos = chunkCenterPos;
                            pos.Y += (i * stairLevel);
                            BlockPos diffDescentLow = SelectedSchematicDescentLower.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                            SelectedSchematicDescentLower.Place(blockAccessor, api.World, diffDescentLow, EnumReplaceMode.ReplaceAllNoAir, true);
                        }
                        else if ((i >= 2) & (i < descentCount))
                        {
                            // api.Server.LogVerboseDebug($"средняя часть");
                            var pos = chunkCenterPos;
                            pos.Y += (i * stairLevel);
                            BlockSchematic[] descentMiddle = schematics[SchematicsType.descentMiddle];
                            var SelectedSchematicDescentMiddle = descentMiddle[rand.Next(0, descentMiddle.Length)];
                            SelectedSchematicDescentMiddle.Init(blockAccessor);
                            BlockPos diffDescentMid = SelectedSchematicDescentMiddle.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                            SelectedSchematicDescentMiddle.Place(blockAccessor, api.World, diffDescentMid, EnumReplaceMode.ReplaceAllNoAir, true);
                        }
                        else if (i == descentCount )
                        {
                            var pos = chunkCenterPos;
                            pos.Y += mapHeight - 12;
                            BlockSchematic[] descentUpper = schematics[SchematicsType.descentUpper];
                            var SelectedSchematicDescentUpper = descentUpper[rand.Next(0, descentUpper.Length)];
                            SelectedSchematicDescentUpper.Init(blockAccessor);
                            BlockPos diffDescentUp = SelectedSchematicDescentUpper.GetStartPos(chunkCenterPos, EnumOrigin.BottomCenter) + schematicCenterPos;
                            SelectedSchematicDescentUpper.Place(blockAccessor, api.World, diffDescentUp, EnumReplaceMode.ReplaceAllNoAir, true);
                        }
                    }
                }
                blockAccessor.Commit();
            }
        }

        private void OnWorldGenBlockAccessor(IChunkProviderThread chunkProvider)
        {
            this.blockAccessor = chunkProvider.GetBlockAccessor(false);
        }

        private SchematicsType SimpleShaftsType(int X, int Z)
        {
            int stepX = 3;
            int stepZ = 12;

            int gapZ = stepZ * 1;

            if ((X % stepX > 0) & (Z % gapZ == 0))
            {
                if (InPlaceablePlace(X, stepX * 1) == true)
                {
                    return SchematicsType.simpleShafts;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }
            else if ((X % stepX == 0) & (Z % gapZ == 0))
            {

                return SchematicsType.horizontalSchematics;
            }
            else if ((Z % stepZ) > 0 & (X % stepX) == 0)
            {
                if (InPlaceablePlace(Z, stepZ * 3) == true)
                {
                    return SchematicsType.simpleShafts;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }
            else if ((Z % stepZ == 0) & (X % stepX == 0))
            {
                return SchematicsType.simpleShafts;
            }
            else
            {
                return SchematicsType.Empty;
            }
        }

        private SchematicsType SelectSchematicTypeXAxis(int X, int Z)
        {
            int stepX = 9;
            int stepZ = 9;

            int gapZ = stepZ * 2;
     
            if ((X % stepX > 0) & (Z % gapZ == 0) )
            {
                if (InPlaceablePlace(X, stepX * 1 ) == true)
                {
                    return SchematicsType.horizontalSchematics;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }

            else if ((X % stepX == 0) & (Z % gapZ == 0))
            {
                
                return SchematicsType.horizontalSchematics;
            }

            else if ((Z % stepZ ) > 0 & (X % stepX ) == 0)
            {
                if (InPlaceablePlace(Z, stepZ * 3) == true)
                {
                    return SchematicsType.verticalSchematics;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }

            else if ((Z % stepZ == 0) & (X % stepX == 0))
            {
                return SchematicsType.angledRightDownSchematics;
            }


            else
            {
                return SchematicsType.Empty;
            }
        }

        private SchematicsType SelectSchematicTypeZAxis(int X, int Z)
        {
            int stepX = 5;
            int stepZ = 5;

            int gapX = stepX * 3;

            if ((Z % stepZ > 0) & (X % gapX == 0))
            {
                if (InPlaceablePlace(Z, stepZ * 2) == true)
                {
                    return SchematicsType.horizontalSchematics;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }

            else if ((Z % stepZ == 0) & (X % gapX == 0))
            {

                return SchematicsType.angledLeftDownSchematics;
            }

            else if ((X % stepX) > 0 & (Z % stepX) == 0)
            {
                if (InPlaceablePlace(X, stepX) == true)
                {
                    return SchematicsType.verticalSchematics;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }

            else if ((X % stepX == 0) & (Z % stepZ == 0))
            {
                return SchematicsType.angledLeftUpSchematics;
            }


            else
            {
                return SchematicsType.Empty;
            }
        }

        private SchematicsType SelectSchematicTypeZAxisAdditional(int X, int Z)
        {
            int stepX = 15;
            int stepZ = 5;

            int gapZ = stepZ;

            if ((X % stepX > 0) & (Z % gapZ == 0))
            {
                if (InPlaceablePlace(X, stepX * 1) == true)
                {
                    return SchematicsType.verticalSchematics;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }

            else if ((X % stepX == 0) & (Z % gapZ == 0)) // case at X and Z axis by both steps ⌟
            {

                return SchematicsType.verticalSchematics;
            }

            else if ((Z % stepZ) > 0 & (X % stepX) == 0)  // case at X step only |
            {
                if (InPlaceablePlace(Z, stepZ * 3) == true)
                {
                    return SchematicsType.horizontalSchematics;
                }
                else
                {
                    return SchematicsType.Empty;
                }
            }

            else if ((Z % stepZ == 0) & (X % stepX == 0)) // case at X step only ↱
            {
                return SchematicsType.angledRightDownSchematics;
            }


            else
            {
                return SchematicsType.Empty;
            }
        }

        private static bool InPlaceablePlace(int pos, int gap)
        {
            if ( pos % gap  % 10 < gap / 2)
            {
                return true;
            }
            return false;
            
        }
    }
}