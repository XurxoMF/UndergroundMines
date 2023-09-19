using System.Collections.Generic;
using ProtoBuf;

namespace UndergroundMines
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SavedData
    {
        /// <summary>
        /// Structure can be null if in that chunk no structure were created.
        /// </summary>
        public Dictionary<Chunk, Structure> GeneratedStructures;

        internal bool Modified;

        public SavedData()
        {
            GeneratedStructures = new Dictionary<Chunk, Structure>();
        }
    }
}