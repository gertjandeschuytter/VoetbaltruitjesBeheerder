using BusinessLayer.Model;
using BusinessLayer_VoetbaltruitjesWinkel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTestVoetbaltruitje {
    public class UnitTestClub {

        private Club _club;

        [Theory()]
        [InlineData("", "")]
        public void Test_Club(string competitie, string ploeg)
        {
            Assert.Throws<VoetbaltruitjeException>(() => _club = new(competitie, ploeg));
        }
    }
}
