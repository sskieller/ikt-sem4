namespace DoorControl
{
	public interface IEntryNotification
	{
		void NotifyEntryGranted();
		void NofifyEntryDenied();
		void SignalAlarm();
	}
}