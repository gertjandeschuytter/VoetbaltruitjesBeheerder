using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Exceptions {
    public class VoetbaltruitjeException : Exception {
        public VoetbaltruitjeException() {
        }

        public VoetbaltruitjeException(string message) : base(message) {
        }

        public VoetbaltruitjeException(string message, Exception innerException) : base(message, innerException) {
        }

        protected VoetbaltruitjeException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
