using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Exceptions {
    public class KlantException : Exception {
        public KlantException() {
        }

        public KlantException(string message) : base(message) {
        }

        public KlantException(string message, Exception innerException) : base(message, innerException) {
        }

        protected KlantException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
