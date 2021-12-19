using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer_VoetbaltruitjesWinkel.ExceptionsForManagers {
    class ClubManagerException : Exception {
        public ClubManagerException() {
        }

        public ClubManagerException(string message) : base(message) {
        }

        public ClubManagerException(string message, Exception innerException) : base(message, innerException) {
        }

        protected ClubManagerException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
