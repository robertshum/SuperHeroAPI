using System.Runtime.Serialization;

namespace SuperHeroAPI.Exceptions
{
    [Serializable]
    internal class SuperHeroNotFoundException : Exception
    {
        public SuperHeroNotFoundException()
        {
        }

        public SuperHeroNotFoundException(string? message) : base(message)
        {
        }

        public SuperHeroNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected SuperHeroNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}