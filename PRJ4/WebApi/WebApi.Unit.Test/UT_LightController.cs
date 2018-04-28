using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using FWPS;
using FWPS.Controllers;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using NUnit.Framework;



namespace WebApi.Unit.Test
{
    [TestFixture]
    public class UT_LightController
    {
        private LightController _lc;
        private LightController _lc2;
        private LightItem _li;
        private LightItem _li2;
        private IHubContext<DevicesHub> _hub;
        private SqliteConnection _connection;
        private DbContextOptions<FwpsDbContext> _options;
        private FwpsDbContext _context;

        public IQueryable IQueryable;

        [SetUp]
        public void SetUp()
        {
            _li = new LightItem
            {
                Command = "What",
                WakeUpTime = DateTime.Parse("2018-03-02 08:00"),
                SleepTime = DateTime.Parse("2018-03-02 20:00"),
                IsRun = true,
                IsOn = true
            };
            _li2 = new LightItem
            {
                Command = "What",
                WakeUpTime = DateTime.Parse("2018-03-02 08:00"),
                SleepTime = DateTime.Parse("2018-03-02 20:00"),
                IsRun = true,
                IsOn = true
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
        public void GetById_IdNotExisting_ExpectedResult_NullReferenceException()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();
                _lc = new LightController(_context, _hub);
                _li.Command = "FirstItem";
                _lc.Create(_li);

                long id = 3;

                var result = _lc.GetById(id) as ObjectResult;

                Assert.That(() => result.Value as LightItem, Throws.Exception.TypeOf<NullReferenceException>());
            }
        }

        [Test]
        public void GetById_IdExist_ExpectedResult_LightItemReturned()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();
                _lc = new LightController(_context, _hub);
                _li.Command = "FirstItem";
                _lc.Create(_li);

                long id = 2;

                var result = _lc.GetById(id) as ObjectResult;
                var model = result?.Value as LightItem;

                Debug.Assert(model != null, nameof(model) + " != null");
                Assert.That(model.Id, Is.EqualTo(id));
            }
        }

        [Test]
        public void Constructor_CreatingFirstLightController_ExpectedResult_LightItemAddedToContext()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                Assert.That(_context.LightItems.Any(), Is.EqualTo(true));
            }
        }

        [Test]
        public void Constructor_CreatingTwoLightControllers_ExpectedResult_OneLightItemAddedOnly()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);
                _lc2 = new LightController(_context, _hub);


                List<LightItem> list = _context.LightItems
                    .ToList();

                Assert.That(list.Count, Is.EqualTo(1));
            }
        }


        [Test]
        public void Create_CreateOneItem_ValidLightItem_ExpectedResult_CountEquals2()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                _lc.Create(_li);

                List<LightItem> list = _context.LightItems.ToList();

                Assert.That(list.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void Create_CreateTwoItems_ValidLightItems_ExpectedResult_CountEquals3()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                _lc.Create(_li);
                _lc.Create(_li2);

                List<LightItem> list = _context.LightItems.ToList();

                Assert.That(list.Count, Is.EqualTo(3));
            }
        }

        [Test]
        public void Create_CreateItem_NullItem_ExpectedResult_NoHubRequestRecieved()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                LightItem nullItem = null;

                _hub.Clients.All.DidNotReceive().InvokeAsync(Arg.Any<string>());

            }
        }

        [Test]
        public void Create_CreateItem_CorrectStringInputFirstArg_ExpectedResult_True()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);
                
                _lc.Create(_li);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Is<string>(firstArg => firstArg == "UpdateSpecific"),
                    Arg.Any<string>(), 
                    Arg.Any<string>(), 
                    Arg.Any<object>());

            }
        }

        [Test]
        public void Create_CreateItem_CorrectStringInputSecondArg_ExpectedResult_True()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                _lc.Create(_li);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Any<string>(),
                    Arg.Is<string>(secondArg => secondArg == "MorningSun"),
                    Arg.Any<string>(),
                    Arg.Any<object>());

            }
        }

        [Test]
        public void Create_CreateItem_CorrectStringInputThirdArg_ExpectedResult_True()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                _li.Command = "ThirdArg";

                _lc.Create(_li);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<string>(thirdArg => thirdArg == "ThirdArg"),
                    Arg.Any<object>());
            }
        }

        [Test]
        public void Create_CreateItem_CorrectObjectInputFourthArg_ExpectedResult_True()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();

                _lc = new LightController(_context, _hub);

                _li.Command = "ThirdArg";

                _lc.Create(_li);

                _hub.Clients.All.Received(1).InvokeAsync(
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<LightItem>(fourthArg => fourthArg == _li)
                    );
            }
        }

        [Test]
        public void Update_UpdateItem_ExpectedResult_CommandUpdated()
        {
            using (_context = new FwpsDbContext(_options))
            {
                long id = 2;
                _li.Id = id;

                _context.Database.EnsureCreated();

                _li2.Command = "SecondItem";
                _lc = new LightController(_context,_hub);

                _lc.Create(_li2);

                _li.Command = "FirstItem";
                _lc.Update(id, _li);

                Assert.That(_context.LightItems.
                    ToList()[1].Command, 
                    Is.EqualTo("FirstItem"));
            }
        }

        [Test]
        public void GetAll_ListOfItems_ExpectedResult_ReturnedView()
        {
            using (_context = new FwpsDbContext(_options))
            {
                _context.Database.EnsureCreated();
                _lc = new LightController(_context, _hub);
                _lc.Create(_li);
                _lc.Create(_li2);

                Assert.That(_lc.GetAll(), Is.EqualTo(_context.LightItems.ToList()));
            }
        }

        //_hub.Clients.All.Received(1).InvokeAsync(Arg.Is<string>( bob => bob == "UpdateSpecific"), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<object>());


        //// In-memory database only exists while the _connection is open
        //var _connection = new SqliteConnection("DataSource=:memory:");
        //_connection.Open();

        //try
        //{
        //    var _options = new DbContextOptionsBuilder<BloggingContext>()
        //        .UseSqlite(_connection)
        //        .Options;

        //    // Create the schema in the database
        //    using (var _context = new BloggingContext(_options))
        //    {
        //        _context.Database.EnsureCreated();
        //    }

        //    // Run the test against one instance of the _context
        //    using (var _context = new BloggingContext(_options))
        //    {
        //        var service = new BlogService(_context);
        //        service.Add("http://sample.com");
        //    }

        //    // Use a separate instance of the _context to verify correct data was saved to database
        //    using (var _context = new BloggingContext(_options))
        //    {
        //        Assert.AreEqual(1, _context.Blogs.Count());
        //        Assert.AreEqual("http://sample.com", _context.Blogs.Single().Url);
        //    }
        //}
        //finally
        //{
        //    _connection.Close();
        //}


    }
}
