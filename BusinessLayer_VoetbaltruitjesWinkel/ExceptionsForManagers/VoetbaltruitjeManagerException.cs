using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers {
    class VoetbaltruitjeManagerException : Exception {
        public VoetbaltruitjeManagerException() {
        }

        public VoetbaltruitjeManagerException(string message) : base(message) {
        }

        public VoetbaltruitjeManagerException(string message, Exception innerException) : base(message, innerException) {
        }

        protected VoetbaltruitjeManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
