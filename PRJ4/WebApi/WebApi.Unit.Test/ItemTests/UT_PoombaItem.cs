using System;
using System.Collections.Generic;
using System.Text;
using FWPS.Models;
using NUnit.Framework;

namespace WebApi.Unit.Test
{
    [TestFixture]
    class UT_PoombaItem
    {
        private PoombaItem _uut;

        private string _command;
        private bool _isRun;
        private List<Room> _rooms;
        private DateTime _cleaningTime;
        private Room _stubRoom;


        [SetUp]
        public void SetUp()
        {
            _stubRoom = new Room()
            {
                RoomName = "Room"
            };

            _command = "What";
            _isRun = true;
            _rooms = new List<Room>(){_stubRoom};
            _cleaningTime = DateTime.Parse("2018-03-04 22:46:23");


            _uut = new PoombaItem()
            {
                CleaningTime = _cleaningTime,
                Command = _command,
                IsRun = _isRun,
                Rooms = _rooms
            };
        }

        [Test]
        public void PoombaItem_AttributeTest_ExpectedResult_True()
        {
            Assert.That(_uut.Command, Is.EqualTo(_command));
            Assert.That(_uut.CleaningTime, Is.EqualTo(_cleaningTime));
            Assert.That(_uut.IsRun, Is.EqualTo(_isRun));
            Assert.That(_uut.Rooms[0].RoomName, Is.EqualTo(_rooms[0].RoomName));
        }
    }
}
