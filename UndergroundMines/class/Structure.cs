using ProtoBuf;

namespace UndergroundMines
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