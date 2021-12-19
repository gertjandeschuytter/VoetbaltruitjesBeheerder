using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER.Exceptions {
    class VoetbaltruitjeRepositoryADOExceptions : Exception {
        public VoetbaltruitjeRepositoryADOExceptions() {
        }

        public VoetbaltruitjeRepositoryADOExceptions(string message) : base(message) {
        }

        public VoetbaltruitjeRepositoryADOExceptions(string message, Exception innerException) : base(message, innerException) {
        }

        protected VoetbaltruitjeRepositoryADOExceptions(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
