using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RouletteGame.Legacy;
using NSubstitute;

namespace Roulette.Unit.Test
{
    [TestFixture]
    public class RouletteUnitTest
    {
        #region FieldTest

        [TestCase(0u, TestName = "Field_Create_LowerBound_Value=0")]
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

        #endregion

        #region BetTest
        // Winning bets

        [TestCase(360, 10u, 32u, Field.Black, TestName = "FieldBet_Winner_Amount=360")]
        [TestCase(0, 0u, 32u, Field.Black, TestName = "FieldBet_Winner_Amount=0")]
        [TestCase(720, 20u, 16u, Field.Black, TestName = "FieldBet_Winner_Amount=720")]
        public void FieldBetTest_Winning(int returnResult, uint amount, uint fieldNumber, uint fieldColor)
        {
            var field = new Field(fieldNumber, fieldColor);
            var uut = new FieldBet("StubBet", amount, fieldNumber);

            Assert.That(returnResult, Is.EqualTo(uut.WonAmount(field)));
        }

        [TestCase(20, 10u, 32u, Field.Black, TestName = "ColorBet_Winner_Amount=20")]
        [TestCase(0, 0u, 32u, Field.Black, TestName = "ColorBet_Winner_Amount=0")]
        [TestCase(110, 55u, 16u, Field.Black, TestName = "ColorBet_Winner_Amount=110")]
        public void ColorBetTest_Winning(int returnResult, uint amount, uint fieldNumber, uint fieldColor)
        {
            var field = new Field(fieldNumber, fieldColor);
            var uut = new ColorBet("StubBet", amount, fieldColor);

            Assert.That(returnResult, Is.EqualTo(uut.WonAmount(field)));
        }

        [TestCase(20, 10u, 32u, Field.Black, TestName = "EvenOddBet_Winner_Amount=20")]
        [TestCase(0, 0u, 32u, Field.Black, TestName = "EvenOddBet_Winner_Amount=0")]
        [TestCase(110, 55u, 16u, Field.Black, TestName = "EvenOddBet_Winner_Amount=110")]
        public void EvenOddBetTest_Winning(int returnResult, uint amount, uint fieldNumber, uint fieldColor)
        {
            var field = new Field(fieldNumber, fieldColor);
            var uut = new EvenOddBet("StubBet", amount, true);

            Assert.That(returnResult, Is.EqualTo(uut.WonAmount(field)));
        }

        // Loosing bets
        [TestCase(0, 10u, 32u, Field.Black, TestName = "FieldBet_Looser_Amount=0")]
        [TestCase(0, 100u, 32u, Field.Black, TestName = "FieldBet_Looser_Amount=0")]
        [TestCase(0, 20u, 16u, Field.Black, TestName = "FieldBet_Looser_Amount=0")]
        public void FieldBetTest_Loosing(int returnResult, uint amount, uint fieldNumber, uint fieldColor)
        {
            var field = new Field(fieldNumber, fieldColor);
            var uut = new FieldBet("StubBet", amount, fieldNumber+1);

            Assert.That(returnResult, Is.EqualTo(uut.WonAmount(field)));
        }

        [TestCase(0, 10u, 32u, Field.Black, TestName = "ColorBet_Looser_Amount=0")]
        [TestCase(0, 0u, 32u, Field.Black, TestName = "ColorBet_Looser_Amount=0")]
        [TestCase(0, 55u, 16u, Field.Black, TestName = "ColorBet_Looser_Amount=0")]
        public void ColorBetTest_Loosing(int returnResult, uint amount, uint fieldNumber, uint fieldColor)
        {
            var field = new Field(fieldNumber, fieldColor);
            var uut = new ColorBet("StubBet", amount, fieldColor+1);

            Assert.That(returnResult, Is.EqualTo(uut.WonAmount(field)));
        }

        [TestCase(0, 10u, 32u, Field.Black, TestName = "EvenOddBet_Looser_Amount=0")]
        [TestCase(0, 10u, 32u, Field.Black, TestName = "EvenOddBet_Looser_Amount=0")]
        [TestCase(0, 55u, 16u, Field.Black, TestName = "EvenOddBet_Looser_Amount=0")]
        public void EvenOddBetTest_Loosing(int returnResult, uint amount, uint fieldNumber, uint fieldColor)
        {
            var field = new Field(fieldNumber, fieldColor);
            var uut = new EvenOddBet("StubBet", amount, false);

            Assert.That(returnResult, Is.EqualTo(uut.WonAmount(field)));
        }

        [Test]
        public void BetTest_Name_Equals_Troes()
        {
            var uut = new EvenOddBet("Troels", 50u, true);

            Assert.That(uut.PlayerName, Is.EqualTo("Troels"));
        }

        #endregion

        #region RouletteTest

        [Test]
        public void RouletteTest_No_Spin_Result_0_Green()
        {
            var uut = new RouletteGame.Legacy.Roulette();
            
            Assert.That(uut.GetResult().Number, Is.EqualTo(new Field(0, Field.Green).Number));
            Assert.That(uut.GetResult().Color, Is.EqualTo(new Field(0, Field.Green).Color));
        }

        [TestCase(8u, 8u, Field.Black, TestName = "Spinning_8_Expect_8_Black")]
        [TestCase(0u, 0u, Field.Green, TestName = "Spinning_0_Expect_0_Green")]
        [TestCase(1u, 1u, Field.Red, TestName = "Spinning_1_Expect_1_Red")]
        [TestCase(35u, 35u, Field.Black, TestName = "Spinning_35_Expect_35_Black")]
        [TestCase(36u, 36u, Field.Red, TestName = "Spinning_36_Expect_36_Red")]
        public void RouletteTest_Spin_GetResult(uint numberToSpin, uint expectedResult, uint expectedColor)
        {
            var uut = new RouletteGame.Legacy.Roulette(new StubRandom(numberToSpin));

            uut.Spin();

            var winnerField = new Field(expectedResult, expectedColor);
            Assert.That(uut.GetResult().Number, Is.EqualTo(winnerField.Number));
            Assert.That(uut.GetResult().Color, Is.EqualTo(winnerField.Color));
        }

        [Test]
        public void RouletteTest_Spin_Has_Been_Called()
        {
            var randomEngine = new RouletteGame.Legacy.Random();
            var uut = new RouletteGame.Legacy.Roulette(randomEngine);

            uut.Spin();

            Assert.That(randomEngine.hasbeenCalled, Is.EqualTo(true));
        }
        #endregion
    }
}
