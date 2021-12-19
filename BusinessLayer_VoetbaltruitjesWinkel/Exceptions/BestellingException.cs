using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.Exceptions {
    public class BestellingException : Exception {
        public BestellingException() {
        }

        public BestellingException(string message) : base(message) {
        }

        public BestellingException(string message, Exception innerException) : base(message, innerException) {
        }

        protected BestellingException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
