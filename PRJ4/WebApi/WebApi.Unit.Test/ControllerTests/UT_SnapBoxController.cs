using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using BadRequestResult = Microsoft.AspNetCore.Mvc.BadRequestResult;
using NotFoundResult = Microsoft.AspNetCore.Mvc.NotFoundResult;

namespace WebApi.Unit.Test.ControllerTests
{
    [TestFixture]
    class UT_SnapBoxController
    {
        private SnapBoxController _snapBoxController;
        private SnapBoxItem _stubSnapBoxItem1;
        private SnapBoxItem _stubSnapBoxItem2;

        private IHubContext<DevicesHub> _hub;
        private SqliteConnection _connection;
        private DbContextOptions<FwpsDbContext> _options;
        private FwpsDbContext _stubContext;

        [SetUp]
        public void SetUp()
        {
            _stubSnapBoxItem1 = new SnapBoxItem()
            {
                Checksum = "12345",
                MailReceived = true,
                PowerLevel = "9001",
                SnapBoxId = "007",
                ReceiverEmail = "bob@bob.com"
            };
            _stubSnapBoxItem2 = new SnapBoxItem()
            {
                Checksum = "54321",
                MailReceived = false,
                PowerLevel = "003",
                SnapBoxId = "03",
                ReceiverEmail = "nobody"
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
            _connection.Close();
        }

        [Test]
        public void Constructor_DefaultItemCreated_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();
                _snapBoxController = new SnapBoxController(_stubContext);
                
                Assert.That(_stubContext.SnapBoxItems.Any(), Is.EqualTo(true));
            }
        }

        [Test]
        public void Constructor_DefaultItem_ExpectedResult_CorrectId()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();
                _snapBoxController = new SnapBoxController(_stubContext);
                
                Assert.That(_stubContext.SnapBoxItems.ToList()[0].SnapBoxId, Is.EqualTo("000"));
            }
        }

        [Test]
        public void Constructor_NullContext_ExpectedResult_ExceptionThrown()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                Assert.That(() => _snapBoxController = new SnapBoxController(null), 
                    Throws.Exception.TypeOf<NullReferenceException>());
            }
        }

        [Test]
        public void Create_BadItem_ExpectedResult_BadRequest()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);

                IActionResult result = _snapBoxController.Create(null);
                
                Assert.IsInstanceOf<BadRequestResult>(result);
            }
        }

        [Test]
        public void Create_PowerlevelNull_ExpectedResult_BadRequest()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);

                _stubSnapBoxItem1.PowerLevel = null;

                IActionResult result = _snapBoxController.Create(_stubSnapBoxItem1);

                Assert.IsInstanceOf<BadRequestObjectResult>(result, "Powerlevel null");
            }
        }

        [Test]
        public void Create_MailReceived_ReceiverMailNull_ExpectedResult_BadRequest()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);

                _stubSnapBoxItem1.MailReceived = true;
                _stubSnapBoxItem1.ReceiverEmail = null;

                IActionResult result = _snapBoxController.Create(_stubSnapBoxItem1);

                Assert.IsInstanceOf<BadRequestObjectResult>(result, "No ReceiverEmail specified");
            }
        }

        [Test]
        public void Create_MailSenderCreatedAndAdded_ExpectedResult_SendSnapBoxMailExecuted_Exception()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                // Has to use a mock controller since a MailSender is created inside the 
                // Create() function, and this needed to be stubbed for testing purposes
                var mockController = new MockSnapBoxController(_stubContext);

                Assert.That(() => mockController.Create(_stubSnapBoxItem1), 
                    Throws.Exception.TypeOf<SendSnapBoxMailExecutedException>());
            }
        }

        [Test]
        public void Create_ValidItemGiven_ExpectedResult_CreatedAtRoute()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);

                IActionResult result = _snapBoxController.Create(_stubSnapBoxItem1);

                Assert.IsInstanceOf<CreatedAtRouteResult>(result);
            }
        }

        [Test]
        public void Delete_OneItemDeleted_ExpectedResult_NoContentResult()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);

                IActionResult result = _snapBoxController.Delete(1);

                Assert.IsInstanceOf<NoContentResult>(result);
            }
        }

        [Test]
        public void Delete_SeveralItemsDeleted_ExpectedResult_NonDeletedItemsStillExists()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);
                _snapBoxController.Create(_stubSnapBoxItem1);
                _snapBoxController.Create(_stubSnapBoxItem2);

                _snapBoxController.Delete(1);
                _snapBoxController.Delete(3);

                var lst = _snapBoxController.GetAll().ToList();

                Assert.That(lst.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Delete_ItemNotExisting_ExpectedResult_NotFoundResult()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);
                _snapBoxController.Delete(1);

                IActionResult result = _snapBoxController.Delete(1);

                Assert.IsInstanceOf<NotFoundResult>(result);
            }
        }

        [Test]
        public void GetAll_ThreeItemsCreated_ExpectedResult_Count3()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);
                _snapBoxController.Create(_stubSnapBoxItem1);
                _snapBoxController.Create(_stubSnapBoxItem2);

                var lst = _snapBoxController.GetAll();

                Assert.That(lst.ToList().Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void GetAll_ZeroItemsInDatabase_ExpectedResult_NoContentResult()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);
                _snapBoxController.Delete(1);

                Assert.That(() => _snapBoxController.GetAll(), Throws.Exception.TypeOf<NoItemsInDatabaseException>());
            }
        }

        [Test]
        public void GetById_GivenId_ExpectedResult_ObjectResult()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);
                _snapBoxController.Create(_stubSnapBoxItem1);

                IActionResult result = _snapBoxController.GetById(2);

                Assert.IsInstanceOf<ObjectResult>(result);
            }
        }

        [Test]
        public void GetById_SpecificAttribute_ExpectedResult_True()
        {
            using (_stubContext = new FwpsDbContext(_options))
            {
                _stubContext.Database.EnsureCreated();

                _snapBoxController = new SnapBoxController(_stubContext);
                _snapBoxController.Create(_stubSnapBoxItem1);

                var result = _snapBoxController.GetById(2) as ObjectResult;
                Debug.Assert(result != null, nameof(result) + " != null");
                var model = result.Value as SnapBoxItem;

                Debug.Assert(model != null, nameof(model) + " != null");
                Assert.That(model.SnapBoxId, Is.EqualTo("007"));
            }
        }
    }
}
