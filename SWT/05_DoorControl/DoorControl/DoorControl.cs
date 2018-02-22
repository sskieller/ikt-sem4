using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoorControl
{
	public class DoorEventArgs : EventArgs
	{
		public bool Forced { get; set; }
	}

	public class DoorControl
    {
	    public event EventHandler DoorOpen;
	    public event EventHandler DoorClosed;

	    private IDoor _door;
	    private IUserValidation _userValidation;
	    private IEntryNotification _entryNotification;
		public DoorControl(IDoor door, IUserValidation userValidation, IEntryNotification entryNotification)
		{
			_door = door;
			_userValidation = userValidation;
			_entryNotification = entryNotification;
		}

	    public void RequestEntry(string id)
	    {
		    if (_userValidation.ValidateEntryRequest(id) == true)
		    {
			    _door.Open();
				_entryNotification.NotifyEntryGranted();
		    }
		    else
		    {
			    _entryNotification.NofifyEntryDenied();
		    }
	    }

	    protected virtual void OnDoorOpen(DoorEventArgs e)
	    {
		    EventHandler handler = DoorOpen;
		    if (DoorOpen != null)
		    {
				DoorOpenHandler(this, e);
		    }
	    }

	    private void DoorOpenHandler(object sender, DoorEventArgs e)
	    {
		    if (e.Forced == true)
		    {
				_entryNotification.SignalAlarm();
		    }
		    else
		    {
				
		    }
	    }
    }
}
