using System;

namespace DoorControl
{
    public class DoorEventArgs : EventArgs
    {
        public bool Forced { get; set; }
    }
    public interface IDoor
	{
		event EventHandler<DoorEventArgs> DoorChangedEvent;
		void Open();
		void Close();
	}
}