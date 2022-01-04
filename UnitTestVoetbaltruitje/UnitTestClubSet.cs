using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestVoetbaltruitje {
    public class UnitTestClubSet {

        private ClubSet _clubSet;

        [Theory()]
        [InlineData(true, 0)]
        public void Test_ClubSet(bool thuis, int ploeg)
        {
            Assert.Throws<VoetbaltruitjeException>(() => _clubSet = new(thuis, ploeg));
        }
    }
}
