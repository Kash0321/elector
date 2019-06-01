using System;
using System.Runtime.Serialization;

namespace Kash.Elector
{
    [Serializable]
    public class ElectorException : Exception
    {
        public ElectorException() { }
        public ElectorException(string message) : base(message) { }
        public ElectorException(string message, Exception inner) : base(message, inner) { }
        protected ElectorException(
          SerializationInfo info,
          StreamingContext context) : base(info, context) { }
    }
}
