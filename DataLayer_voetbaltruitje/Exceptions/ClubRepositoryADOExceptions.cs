using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.DATALAYER.Exceptions {
    class ClubRepositoryADOExceptions : Exception {
        public ClubRepositoryADOExceptions() {
        }

        public ClubRepositoryADOExceptions(string message) : base(message) {
        }

        public ClubRepositoryADOExceptions(string message, Exception innerException) : base(message, innerException) {
        }

        protected ClubRepositoryADOExceptions(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
