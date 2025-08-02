using ProtoBuf;
using UndergroundMines.enums;

namespace UndergroundMines.classes
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Structure
    {
        public ESchematicType Type;
        public ERotation Rotation;

        public Structure(ESchematicType type, ERotation rotation)
        {
            Type = type;
            Rotation = rotation;
        }

        public Structure()
        {
        }
    }
}