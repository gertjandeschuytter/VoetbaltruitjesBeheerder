using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers {
    class BestellingManagerException : Exception {
        public BestellingManagerException() {
        }

        public BestellingManagerException(string message) : base(message) {
        }

        public BestellingManagerException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BestellingManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
