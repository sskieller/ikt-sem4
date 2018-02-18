using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Runtime.CompilerServices;
using ECS.Legacy;

namespace ECS.Unit.Test
{
    [TestFixture]
    public class ECSUnitTests
    {
        [Test]
        public void TestGetCurrentTemp_Return25()
        {
            var uut = new Legacy.ECS(
                25, new FakeTemp(), new FakeHeater());
            Assert.That(uut.GetCurTemp(), Is.EqualTo(25));
        }


        [Test]
        public void TestGetThreshold_Returns25()
        {
            var uut = new Legacy.ECS(
                25, new FakeTemp(), new FakeHeater());

            Assert.That(uut.GetThreshold(), Is.EqualTo(25));
        }

        [TestCase(10, 10, TestName = "TestGetAndSet == 10")]
        public void TestGetAndSetThreshold(int a, int b)
        {
            var uut = new Legacy.ECS(
                25, new FakeTemp(), new FakeHeater());

            Assert.That(uut.GetThreshold(), Is.EqualTo(25));
        }

        [Test]
        public void TestSelfTest_Return_OK()
        {
            var uut = new Legacy.ECS(
                25, new FakeTemp(), new FakeHeater());

            Assert.That(uut.RunSelfTest, Is.EqualTo(true));
        }


    }
    internal class FakeTemp : ITempSensor
    {
        public int GetTemp()
        {
            return 25;
        }

        public bool RunSelfTest()
        {
            return true;
        }
    }

    internal class FakeHeater : IHeater
    {
        public void TurnOn()
        {
            // Do nothing
        }

        public void TurnOff()
        {
            // Do nothing
        }

        public bool RunSelfTest()
        {
            return true;
        }
    }
}
