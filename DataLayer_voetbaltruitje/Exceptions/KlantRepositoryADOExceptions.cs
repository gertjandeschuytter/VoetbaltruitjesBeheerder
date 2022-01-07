using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER.Exceptions {
    public class KlantRepositoryADOExceptions : Exception {
        public KlantRepositoryADOExceptions() {
        }

        public KlantRepositoryADOExceptions(string message) : base(message) {
        }

        public KlantRepositoryADOExceptions(string message, Exception innerException) : base(message, innerException) {
        }

        protected KlantRepositoryADOExceptions(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
