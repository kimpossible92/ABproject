using System;

namespace SnakeMaze.Exceptions
{
    public class NotEnumTypeSupportedException : Exception
    {
        public NotEnumTypeSupportedException() : base(string.Format("Enum value not supported")) { }
    }
}