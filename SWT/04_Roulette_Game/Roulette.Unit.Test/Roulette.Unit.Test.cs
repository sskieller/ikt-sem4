using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RouletteGame.Legacy;

namespace Roulette.Unit.Test
{
    [TestFixture]
    public class RouletteUnitTest
    {
        [TestCase(0u, TestName="Field_Create_LowerBound_Value=0")]
        [TestCase(1u, TestName = "Field_Create_LowerBound_Value=1")]
        [TestCase(36u, TestName = "Field_Create_UpperBound_Value=36")]
        public void AssertFieldCreated(uint a)
        {
            var uut = new Field(a, Field.Black);
            Assert.That(uut.Number, Is.EqualTo(a));
        }

        [TestCase(10u, 0u, TestName = "Field_Create_Color_Red")]
        [TestCase(10u, 1u, TestName = "Field_Create_Color_Black")]
        [TestCase(10u, 2u, TestName = "Field_Create_Color_Green")]
        public void AssertFieldCreated(uint a, uint b)
        {
            var uut = new Field(a, b);
            Assert.That(uut.Color, Is.EqualTo(b));
        }

        [TestCase(10u, true, TestName = "Field_Create_Value=10_IsEven")]
        [TestCase(22u, true, TestName = "Field_Create_Value=22_IsEven")]
        [TestCase(11u, false, TestName = "Field_Create_Value=11_IsOdd")]
        [TestCase(33u, false, TestName = "Field_Create_Value=33_IsOdd")]
        public void AssertFieldCreated(uint a, bool b)
        {
            var uut = new Field(a, Field.Black);
            Assert.That(uut.Even, Is.EqualTo(b));
        }

        [Test]
        public void Field_Create_OutOfBound_Value_37_Expect_Exception()
        {
            Assert.Throws<FieldException>(() => new Field(37, Field.Black));
        }

        [Test]
        public void Field_Create_OutOfBounds_Color_Expect_Exception()
        {
            Assert.Throws<FieldException>(() => new Field(36, 3));
        }
    }
}
