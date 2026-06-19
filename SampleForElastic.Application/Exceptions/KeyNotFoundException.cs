using SampleForElastic.Common.Base;

namespace SampleForElastic.Application.Exceptions
{
    public class KeyNotFoundException : KeyNotFoundApplicationException
    {
        public KeyNotFoundException() : base("Key not found") { }
    }
}
