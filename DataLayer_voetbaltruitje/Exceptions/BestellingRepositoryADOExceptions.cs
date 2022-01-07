using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER.Exceptions {
    public class BestellingRepositoryADOExceptions : Exception {
        public BestellingRepositoryADOExceptions() {
        }

        public BestellingRepositoryADOExceptions(string message) : base(message) {
        }

        public BestellingRepositoryADOExceptions(string message, Exception innerException) : base(message, innerException) {
        }

        protected BestellingRepositoryADOExceptions(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
