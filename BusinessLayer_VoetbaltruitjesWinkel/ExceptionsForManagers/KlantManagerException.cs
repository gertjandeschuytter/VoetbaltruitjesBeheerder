using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers {
    public class KlantManagerException : Exception {
        public KlantManagerException() {
        }

        public KlantManagerException(string message) : base(message) {
        }

        public KlantManagerException(string message, Exception innerException) : base(message, innerException) {
        }

        protected KlantManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
