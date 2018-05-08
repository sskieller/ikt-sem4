using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FWPS;
using FWPS.Controllers;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace WebApi.Unit.Test.ControllerTests
{
    [TestFixture]
    class UT_HodoorController
    {
        private HodoorController _hodoorController;
        private HodoorItem _stubHodoorItem1;
        private HodoorItem _stubHodoorItem2;

        private SqliteConnection _connection;
        private DbContextOptions<FwpsDbContext> _options;
        private FwpsDbContext _stubContext;

        private IHubContext<DevicesHub> _hub;

        [SetUp]
        public void SetUp()
        {
            _stubHodoorItem1 = new HodoorItem()
            {
                Command = "What",
                OpenStatus = true,
                IsRun = false
            };
            _stubHodoorItem2 = new HodoorItem()
            {
                Command = "What",
                OpenStatus = false,
                IsRun = true
            };

            _hub = Substitute.For<IHubContext<DevicesHub>>();

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<FwpsDbContext>()
                .UseSqlite(_connection)
                .EnableSensitiveDataLogging()
                .Options;
        }

        [TearDown]
        public void TearDown()
        {
            _connection?.Close();

        }

        [Test]
        public void Constructor_HubIsNull_ExpectedResult_NullReferenceException()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                Assert.That(() => _hodoorController = new HodoorController(_stubContext, null), 
                    Throws.Exception.TypeOf<NullReferenceException>());
            }
        }

        [Test]
        public void Constructor_DefaultItemCreated_ExpectedResult_NotEmpty()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                Assert.That(_stubContext.HodoorItems, Is.Not.Empty);
            }
        }

        [Test]
        public void Constructor_TwoControllersCreated_ExpectedResult_CountIs1()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);
                HodoorController hodoorControllerTwo = new HodoorController(_stubContext,_hub);
                
                Assert.That(_stubContext.HodoorItems.ToList().Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Create_HodoorItemNull_ExpectedResult_BadRequestResult()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                var result = _hodoorController.Create(null);

                Assert.IsInstanceOf<BadRequestResult>(result);
            }
        }

        [Test]
        public void Create()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                var mockHodoorController = new MockHodoorController(_stubContext, _hub);

                Assert.That(() => mockHodoorController.Create(_stubHodoorItem1),
                    Throws.Exception.TypeOf<WriteExecutedException>());
            }
        }

        

        [Test]
        public void GetAll_()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);
                
                Assert.That(false);
            }
        }
    }
}
