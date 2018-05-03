using System;
using System.Collections.Generic;
using System.Text;
using MasterApplication.Threads;
using NSubstitute;
using NUnit.Framework;
using RabbitMQ.Client;

namespace MasterApplication.Unit.Test
{
    [TestFixture]
    class UT_FwpsListener
    {
        private FwpsListener _listener;
        private IConnection _connection;
        [SetUp]
        public void SetUp()
        {
            
            //_connection = Substitute.For<IConnection>();
            //_listener = new FwpsListener(_connection);
        }

        [Test]
        public void Add_AddTopic_TopicAdded()
        {
            Assert.That(true);
        }
        
    }
}
