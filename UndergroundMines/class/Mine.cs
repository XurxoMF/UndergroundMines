using ProtoBuf;

namespace UndergroundMines
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Mine
    {
        public EnumSchematicsType Type;
        public EnumRotation Rotation;

        public Mine(EnumSchematicsType type, EnumRotation rotation)
        {
            Type = type;
            Rotation = rotation;
        }

        public Mine()
        {
        }
    }
}