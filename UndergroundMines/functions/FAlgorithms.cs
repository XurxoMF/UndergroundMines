using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Server;
using Vintagestory.ServerMods;

namespace UndergroundMines
{
    public class FAlgorithms
    {
        /// <summary>Checks if the given structure has exit in the given side.</summary>
        /// <param name="structure">Structure to look for exits.</param>
        /// <param name="side">Side of the exit.</param>
        /// <returns>true if has exit or false if not.</returns>
        public static bool HasExitInSide(Structure structure, ERotation side)
        {
            if (structure.Type == ESchematicType.UndergroundCross)
            { // exit all sides always true
                return true;
            }

            if (structure.Type == ESchematicType.UndergroundEnd)
            { // exit only one side
                if (structure.Rotation == side) return true;
            }

            if (structure.Type == ESchematicType.UndergroundMine)
            { // exit opossite sides
                if (structure.Rotation == ERotation.North || structure.Rotation == ERotation.South)
                {
                    if (side == ERotation.North || side == ERotation.South)
                    {
                        return true;
                    }
                }
                else
                {
                    if (side == ERotation.East || side == ERotation.West)
                    {
                        return true;
                    }
                }
            }

            if (structure.Type == ESchematicType.UndergroundAngle)
            { // exit N-E, E-S, S-W or W-N
                if (structure.Rotation == ERotation.North)
                {
                    if (side == ERotation.North || side == ERotation.East)
                    {
                        return true;
                    }
                }

                if (structure.Rotation == ERotation.East)
                {
                    if (side == ERotation.South || side == ERotation.East)
                    {
                        return true;
                    }
                }

                if (structure.Rotation == ERotation.South)
                {
                    if (side == ERotation.South || side == ERotation.West)
                    {
                        return true;
                    }
                }

                if (structure.Rotation == ERotation.West)
                {
                    if (side == ERotation.West || side == ERotation.North)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Checks if the N-E-S-W chunks in the side of the given chunk have exit.<br/>
        /// No chunk registered = true | Structure with exit = true | No structure generated = false</summary>
        /// <param name="chunk">Chunk to check sides.</param>
        /// <param name="data">SavedData to look for the created structure and orientation.</param>
        /// <param name="distance">Number of chunks away of the original chunk. 1 is the inmediatly next chunk.</param>
        /// <returns>List with the sides with exit.</returns>
        public static List<ERotation> CheckExitSides(int chunkX, int chunkZ, int chunkSize, int seaLevel, SavedData data, int distance)
        {
            List<ERotation> exits = new();
            Chunk newChunk;

            // north chunk
            newChunk = FChunk.GetChunk(chunkX, chunkZ - distance, chunkSize, seaLevel);
            if (!data.GeneratedStructures.ContainsKey(newChunk))
            {
                exits.Add(ERotation.North);
            }
            else
            {
                var structure = data.GeneratedStructures[newChunk];
                if (structure != null && HasExitInSide(structure, ERotation.South))
                {
                    exits.Add(ERotation.North);
                }
            }

            // east chunk
            newChunk = FChunk.GetChunk(chunkX + distance, chunkZ, chunkSize, seaLevel);
            if (!data.GeneratedStructures.ContainsKey(newChunk))
            {
                exits.Add(ERotation.East);
            }
            else
            {
                var structure = data.GeneratedStructures[newChunk];
                if (structure != null && HasExitInSide(structure, ERotation.West))
                {
                    exits.Add(ERotation.East);
                }
            }

            // south chunk
            newChunk = FChunk.GetChunk(chunkX, chunkZ + distance, chunkSize, seaLevel);
            if (!data.GeneratedStructures.ContainsKey(newChunk))
            {
                exits.Add(ERotation.South);
            }
            else
            {
                var structure = data.GeneratedStructures[newChunk];
                if (structure != null && HasExitInSide(structure, ERotation.North))
                {
                    exits.Add(ERotation.South);
                }
            }

            // west chunk
            newChunk = FChunk.GetChunk(chunkX - distance, chunkZ, chunkSize, seaLevel);
            if (!data.GeneratedStructures.ContainsKey(newChunk))
            {
                exits.Add(ERotation.West);
            }
            else
            {
                var structure = data.GeneratedStructures[newChunk];
                if (structure != null && HasExitInSide(structure, ERotation.East))
                {
                    exits.Add(ERotation.West);
                }
            }

            return exits;
        }

        /// <summary>Checks if the chunks located in the given sides have structures generated.<br/>
        /// Structure = true | No structure = false | Chunk not present = false</summary>
        /// <param name="chunk">Chunk to check sides.</param>
        /// <param name="sides">Array with the sides to check.</param>
        /// <param name="data">SavedData to look for structures.</param>
        /// <returns>List with the sides with structures.</returns>
        public static List<ERotation> CheckStructuredSides(int chunkX, int chunkZ, int chunkSize, int seaLevel, List<ERotation> sides, SavedData data)
        {
            HashSet<ERotation> structures = new();

            foreach (var side in sides)
            {
                // north chunk
                if (side == ERotation.North)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX, chunkZ - 1, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null)
                        {
                            structures.Add(ERotation.North);
                        }
                    }
                }

                // east chunk
                if (side == ERotation.East)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX + 1, chunkZ, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null)
                        {
                            structures.Add(ERotation.East);
                        }
                    }
                }

                // south chunk
                if (side == ERotation.South)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX, chunkZ + 1, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null)
                        {
                            structures.Add(ERotation.South);
                        }
                    }
                }

                // west chunk
                if (side == ERotation.West)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX - 1, chunkZ, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null)
                        {
                            structures.Add(ERotation.West);
                        }
                    }
                }
            }

            return structures.ToList();
        }

        /// <summary>Adjusts the rotation of a structure type to fit exits with the given sides.</summary>
        /// <param name="type">Type of the structure to rotate.</param>
        /// <param name="sides">Sides where the structure must have exits.<br/>List can only contain the sides where it should have exit.</param>
        /// <returns>The Structure with the given type and the acording rotation.</returns>
        public static Structure GetStructureWithAdjustedRotation(ESchematicType type, List<ERotation> sides)
        {
            if (type == ESchematicType.UndergroundEnd)
            { // Only one exit structures.
                int rand = new Random().Next(sides.Count - 1);
                return new Structure(type, sides[rand]);
            }

            if (type == ESchematicType.UndergroundMine)
            { // Only 2 exits in oposite directions.
                // No matter if it's N-S, S-N, W-E or E-W, both sides will be opposite so rotate it 180º or not
                // it's the same.
                if (sides.Count < 1 || sides.Count > 2) return null;
                return new Structure(type, sides[0]);
            }

            // * Can be optimized removing some &&, but for now I'll keep it like this to prevent errors
            if (type == ESchematicType.UndergroundAngle)
            { // Only 2 exits but in angle
                if (sides.Count != 2) return null;

                if (sides.Contains(ERotation.North) && sides.Contains(ERotation.East))
                    return new Structure(type, ERotation.North);

                if (sides.Contains(ERotation.East) && sides.Contains(ERotation.South))
                    return new Structure(type, ERotation.East);

                if (sides.Contains(ERotation.South) && sides.Contains(ERotation.West))
                    return new Structure(type, ERotation.South);

                if (sides.Contains(ERotation.West) && sides.Contains(ERotation.North))
                    return new Structure(type, ERotation.West);
            }

            return null;
        }

        /// <summary>Check if the 2 given sides are opposite of each others or not.</summary>
        /// <param name="sides">List with 2 sides to check.</param>
        /// <returns>true if they are opposite(N-S or W-E) of each other or false if they next to each other(N-E, E-S, S-W or W-N).</returns>
        public static bool AreSidesOpposite(List<ERotation> sides)
        {
            if ((sides.Contains(ERotation.North) && sides.Contains(ERotation.South)) ||
                (sides.Contains(ERotation.West) && sides.Contains(ERotation.East))) return true;
            return false;
        }

        /// <summary>Finds the missing side in a List with 3 sides.</summary>
        /// <param name="sides">List with 3 sides.</param>
        /// <returns>Missing side.</returns>
        public static ERotation GetOppositeSide(ERotation side)
        {
            if (side == ERotation.North) return ERotation.South;
            if (side == ERotation.East) return ERotation.West;
            if (side == ERotation.South) return ERotation.North;
            return ERotation.East;
        }

        /// <summary>Gets a random angle side from the sides List that can be used to place an angle from mainSide.</summary>
        /// <param name="mainSide">Side where angle has to start.</param>
        /// <param name="sides">List of sides to get the angle one. At least one side has to be angle of the main one.</param>
        /// <returns>List with 2 sides in angle.</returns>
        public static List<ERotation> GetRandomAngleSideFromList(ERotation mainSide, List<ERotation> sides)
        {
            List<ERotation> res = new() { mainSide };

            if (mainSide == ERotation.North || mainSide == ERotation.South)
            {
                // Return west or east
                if (sides.Contains(ERotation.West) && sides.Contains(ERotation.East))
                {
                    res.Add(new Random().Next(1) switch
                    {
                        0 => ERotation.West,
                        1 => ERotation.East,
                        _ => ERotation.West
                    });
                }
                else if (sides.Contains(ERotation.West))
                {
                    res.Add(ERotation.West);
                }
                else
                {
                    res.Add(ERotation.East);
                }
            }
            else
            {
                // Return north or south
                if (sides.Contains(ERotation.North) && sides.Contains(ERotation.South))
                {
                    res.Add(new Random().Next(1) switch
                    {
                        0 => ERotation.North,
                        1 => ERotation.South,
                        _ => ERotation.North
                    });
                }
                else if (sides.Contains(ERotation.North))
                {
                    res.Add(ERotation.North);
                }
                else
                {
                    res.Add(ERotation.South);
                }
            }

            return res;
        }

        public static List<ERotation> ColindantToACross(int chunkX, int chunkZ, int chunkSize, int seaLevel, List<ERotation> sides, SavedData data)
        {
            HashSet<ERotation> res = new();

            foreach (var side in sides)
            {
                // north chunk
                if (side == ERotation.North)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX, chunkZ - 1, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null && structure.Type == ESchematicType.UndergroundCross)
                        {
                            res.Add(ERotation.North);
                        }
                    }
                }

                // east chunk
                if (side == ERotation.East)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX + 1, chunkZ, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null && structure.Type == ESchematicType.UndergroundCross)
                        {
                            res.Add(ERotation.East);
                        }
                    }
                }

                // south chunk
                if (side == ERotation.South)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX, chunkZ + 1, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null && structure.Type == ESchematicType.UndergroundCross)
                        {
                            res.Add(ERotation.South);
                        }
                    }
                }

                // west chunk
                if (side == ERotation.West)
                {
                    Chunk newChunk = FChunk.GetChunk(chunkX - 1, chunkZ, chunkSize, seaLevel);
                    if (data.GeneratedStructures.ContainsKey(newChunk))
                    {
                        Structure structure = data.GeneratedStructures[newChunk];
                        if (structure != null && structure.Type == ESchematicType.UndergroundCross)
                        {
                            res.Add(ERotation.West);
                        }
                    }
                }
            }

            return res.ToList();
        }

        // CHOOSE RANDOM STRUCTURES

        public static ESchematicType REndOrNull()
        {
            return new Random().NextDouble() switch
            {
                <= 0.05 => ESchematicType.UndergroundEnd, // 10%
                <= 1.0 => ESchematicType.Null, // 90%
                _ => ESchematicType.Null
            };
        }

        /// <summary>Random structure among UndergroundAngle, UndergroundMine and UndergroundCross.</summary>
        /// <returns>The wining structure type.</returns>
        public static ESchematicType RAngleOrMineOrCross()
        {
            return new Random().NextDouble() switch
            {
                <= 0.3 => ESchematicType.UndergroundMine, // 30%
                <= 0.5 => ESchematicType.UndergroundAngle, // 20%
                <= 1.0 => ESchematicType.UndergroundCross, // 50%
                _ => ESchematicType.UndergroundCross // Default Cross, in case of error I have 4 exits XD
            };
        }

        /// <summary>Random structure between UndergroundAngle and UndergroundMine.</summary>
        /// <returns>The wining structure type.</returns>
        public static ESchematicType RAngleOrMine()
        {
            return new Random().NextDouble() switch
            {
                <= 0.7 => ESchematicType.UndergroundMine, // 70%
                <= 1 => ESchematicType.UndergroundAngle, // 30%
                _ => ESchematicType.UndergroundMine
            };
        }

        /// <summary>Random structure between UndergroundCross and UndergroundMine.</summary>
        /// <returns>The wining structure type.</returns>
        public static ESchematicType RCrossOrMine()
        {
            return new Random().NextDouble() switch
            {
                <= 0.4 => ESchematicType.UndergroundMine, // 40%
                <= 1 => ESchematicType.UndergroundCross, // 60%
                _ => ESchematicType.UndergroundMine
            };
        }

        /// <summary>Random structure between UndergroundCross and UndergroundAngle.</summary>
        /// <returns>The wining structure type.</returns>
        public static ESchematicType RCrossOrAngle()
        {
            return new Random().NextDouble() switch
            {
                <= 0.4 => ESchematicType.UndergroundAngle, // 40%
                <= 1 => ESchematicType.UndergroundCross, // 60%
                _ => ESchematicType.UndergroundMine
            };
        }
    }
}