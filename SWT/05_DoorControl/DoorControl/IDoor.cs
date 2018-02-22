using System;

namespace DoorControl
{
	public interface IDoor
	{
		event EventHandler DoorChangedEvent;
		void Open();
		void Close();
	}
}