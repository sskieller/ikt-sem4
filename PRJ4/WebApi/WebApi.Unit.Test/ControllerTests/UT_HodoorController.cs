using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                IsRun = true
            };
            _stubHodoorItem2 = new HodoorItem()
            {
                Command = "When",
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
        public void Create_DebugWriter_ExpectedResult_WriteExecutedException()
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
        public void Create_NullItem_ExpectedResult_NoHubRequestReceived()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Create(_stubHodoorItem1);

                _hub.Clients.All.DidNotReceive().InvokeAsync(Arg.Any<string>());

            }
        }

        [Test]
        public void Create_HubCreateItem_CorrectStringInputFirstArg_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Create(_stubHodoorItem1);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Is<string>(firstArg => firstArg == "UpdateSpecific"),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<object>());
            }
        }

        [Test]
        public void Create_HubCreateItem_CorrectStringInputSecondArg_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Create(_stubHodoorItem1);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Any<string>(),
                    Arg.Is<string>(secondArg => secondArg == "Hodoor"),
                    Arg.Any<string>(),
                    Arg.Any<object>());
            }
        }

        [Test]
        public void Create_HubCreateItem_CorrectStringInputThirdArg_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _stubHodoorItem1.Command = "ThirdArg";

                _hodoorController.Create(_stubHodoorItem1);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<string>(thirdArg => thirdArg == "ThirdArg"),
                    Arg.Any<object>());
            }
        }

        [Test]
        public void Create_HubCreateItem_CorrectStringInputFourthArg_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);
                
                _hodoorController.Create(_stubHodoorItem1);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<HodoorItem>(fourthArg => fourthArg == _stubHodoorItem1));
            }
        }

        [Test]
        public void GetAll_ThreeItemsInContext_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Create(_stubHodoorItem1);
                _hodoorController.Create(_stubHodoorItem2);


                Assert.That(_hodoorController.GetAll().ToList().Count, Is.EqualTo(_stubContext.HodoorItems.ToList().Count));
            }
        }

        [Test]
        public void GetAll_ZeroItemsInContext_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Delete(1);

                Assert.That(_hodoorController.GetAll().ToList().Count, Is.EqualTo(_stubContext.HodoorItems.ToList().Count));
            }
        }

        [Test]
        public void Next_NoItems_ExpectedResult_NotFound()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();
                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Delete(1);

                IActionResult result = _hodoorController.Next();

                Assert.IsInstanceOf<NotFoundResult>(result);
            }
        }

        [Test]
        public void Next_ThreeItemsInContext_ExpectedResult_FirstItemOnly()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _hodoorController = new HodoorController(_stubContext, _hub);

                _hodoorController.Create(_stubHodoorItem1);
                _hodoorController.Create(_stubHodoorItem2);

                var result = _hodoorController.Next() as ObjectResult;
                var model = result.Value as HodoorItem;

                Assert.That(model.Command, Is.EqualTo("CmdUnlock"));
                Assert.That(model.Command, Is.EqualTo("CmdUnlock"));
            }
        }

        [Test]
        public void Newest_ThreeItemsInContext_ExpectedResult_LastItemOnly()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();
                _hodoorController = new HodoorController(_stubContext, _hub);
                _hodoorController.Create(_stubHodoorItem1);
                _hodoorController.Create(_stubHodoorItem2);

                var result = _hodoorController.Newest() as ObjectResult;
                Debug.Assert(result != null, nameof(result) + " != null");
                var model = result.Value as HodoorItem;

                Debug.Assert(model != null, nameof(model) + " != null");
                Assert.That(model.Command, Is.EqualTo("When"));
                Assert.That(model.Command, Is.EqualTo("When"));
            }
        }

        [Test]
        public void GetById_DefaultItem_ExpectedResult_CommandCmdUnlock()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();
                _hodoorController = new HodoorController(_stubContext, _hub);

                const long id = 1;

                var result = _hodoorController.GetById(id) as ObjectResult;
                var model = result.Value as HodoorItem;

                Assert.That(model.Id, Is.EqualTo(id));
            }
        }
    }
}
