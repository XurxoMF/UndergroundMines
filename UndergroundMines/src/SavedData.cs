using System.Collections.Generic;
using ProtoBuf;

namespace UndergroundMines
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SavedData
    {
        public Dictionary<ChunkPos, Mine> GeneratedMines;

        internal bool Modified;

        public SavedData()
        {
            GeneratedMines = new Dictionary<ChunkPos, Mine>();
        }
    }
}