using System;

namespace thZero.Data.Repository
{
    [Serializable]
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException() : base() { }
        public RepositoryNotFoundException(Type type) : base(type != null ? type.FullName : string.Empty) { }
        public RepositoryNotFoundException(string message) : base(message) { }
        public RepositoryNotFoundException(string message, Exception inner) : base(message, inner) { }
#pragma warning disable CS0628 // New protected member declared in sealed class
        protected RepositoryNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
#pragma warning restore CS0628 // New protected member declared in sealed class
    }
}
