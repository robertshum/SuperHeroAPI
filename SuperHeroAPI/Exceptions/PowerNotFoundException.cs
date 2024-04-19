using System.Runtime.Serialization;

namespace SuperHeroAPI.Exceptions
{
    [Serializable]
    internal class PowerNotFoundException : Exception
    {
        public PowerNotFoundException()
        {
        }

        public PowerNotFoundException(string? message) : base(message)
        {
        }

        public PowerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected PowerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}