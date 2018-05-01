using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace WebApi.Unit.Test.ItemTests
{
    [TestFixture]
    class UT_Room
    {
        private string _roomName;
        private Room _uut;


        [SetUp]
        public void SetUp()
        {
            _roomName = "Room";
            _uut = new Room()
            {
                RoomName = _roomName
            };
        }

        [Test]
        public void Room_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.RoomName, Is.EqualTo(_roomName));
        }
    }
}
