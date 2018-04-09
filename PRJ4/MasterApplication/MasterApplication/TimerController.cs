using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;


namespace MasterApplication
{
    public static class LightTimerController
    {
	    private static readonly System.Timers.Timer _wakeUpTimer;
	    private static readonly System.Timers.Timer _sleepTimer;
		private static TimeSpan _dueTime = TimeSpan.FromMilliseconds(-1);
	    public static EventHandler OnWakeUpTime;
	    public static EventHandler OnSleepTime;

	    public static bool IsRunning => _wakeUpTimer.Enabled;

	    static LightTimerController()
	    {
		    _wakeUpTimer = new Timer();
		    _wakeUpTimer.Elapsed += WakeupOnElapsed;
			_wakeUpTimer.AutoReset = false;
		    _sleepTimer = new Timer();
		    _sleepTimer.Elapsed += SleepTimeOnElapsed;
		    _sleepTimer.AutoReset = false;
		}

	    public static void Start()
	    {
		    if (_wakeUpTimer.Enabled || _dueTime.Milliseconds == -1)
			    return;

			_wakeUpTimer.Start();
	    }

	    public static void Stop()
	    {
		    if (!_wakeUpTimer.Enabled)
			    return;

			_wakeUpTimer.Stop();
	    }
		
	    /// <summary>
	    /// Sets new execution time for timer
	    /// </summary>
	    /// <returns>void</returns>
		public static void SetTime(DateTime whenToHandle)
	    {
		    _dueTime = whenToHandle - DateTime.Now;

		    _wakeUpTimer.Interval = _dueTime.Milliseconds;
	    }

	    private static void WakeupOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
	    {
		    OnWakeUpTime?.Invoke(new Object(), new EventArgs());
	    }
	    private static void SleepTimeOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
	    {
		    OnWakeUpTime?.Invoke(new Object(), new EventArgs());
	    }
	}
}
