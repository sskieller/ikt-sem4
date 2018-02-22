using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using NSubstitute.Core;

namespace DoorControl.Unit.Test
{
	[TestFixture]
    public class DoorControlUnitTest
	{
		private DoorControl _doorControl;
		private IDoor _fakeDoor;
		private IEntryNotification _fakeNotification;
		private IUserValidation _fakeUserValidation;
		
		[SetUp]
	    public void Setup()
	    {
		    _fakeDoor = Substitute.For<IDoor>();
		    _fakeNotification = Substitute.For<IEntryNotification>();
		    _fakeUserValidation = Substitute.For<IUserValidation>();
		    _doorControl = new DoorControl(_fakeDoor, _fakeUserValidation, _fakeNotification);
	    }

	    [Test]
	    public void RequestEntry_ValidId_ExpectAccessGranted()
	    {
		    _fakeUserValidation.ValidateEntryRequest(Arg.Any<string>()).Returns(true);
			_doorControl.RequestEntry(Arg.Any<string>());
			_fakeNotification.Received().NotifyEntryGranted();
	    }

		[Test]
		public void RequestEntry_InvalidId_ExpectAccessDenied()
		{
			_fakeUserValidation.ValidateEntryRequest(Arg.Any<string>()).Returns(false);
			_doorControl.RequestEntry(Arg.Any<string>());
			_fakeNotification.Received().NofifyEntryDenied();
		}

		[Test]
		public void RequestEntry_ValidId_ExpectDoorOpen()
		{
			_fakeUserValidation.ValidateEntryRequest(Arg.Any<string>()).Returns(true);
			_doorControl.RequestEntry(Arg.Any<string>());
			_fakeDoor.Received().Open();
		}
		[Test]
		public void RequestEntry_ValidId_ExpectNoDoorClosed()
		{
			_fakeUserValidation.ValidateEntryRequest(Arg.Any<string>()).Returns(true);
			_doorControl.RequestEntry(Arg.Any<string>());
			_fakeDoor.DidNotReceive().Close();
		}

		[Test]
		public void OnDoorOpen_ForcedEntry_ExpectSignalAlarm()
		{

		}

    }
}
