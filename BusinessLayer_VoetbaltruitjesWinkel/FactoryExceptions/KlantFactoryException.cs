using System;
using System.Runtime.Serialization;

namespace BusinessLayer_VoetbaltruitjesWinkel.Tools {
    [Serializable]
    internal class KlantFactoryException : Exception {
        public KlantFactoryException() {
        }

        public KlantFactoryException(string message) : base(message) {
        }

        public KlantFactoryException(string message, Exception innerException) : base(message, innerException) {
        }

        protected KlantFactoryException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}