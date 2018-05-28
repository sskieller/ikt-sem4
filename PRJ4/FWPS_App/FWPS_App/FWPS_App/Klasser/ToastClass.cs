using Plugin.Toasts;
using Plugin.Toasts.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FWPS_App
{
    class ToastClass
    {
        public void ShowToast(INotificationOptions options)
        {
            var notificator = DependencyService.Get<IToastNotificator>();

            // await notificator.Notify(options);

            notificator.Notify((INotificationResult result) =>
            {
                System.Diagnostics.Debug.WriteLine("Notification [" + result.Id + "] Result Action: " + result.Action);
            }, options);
        }
    }
}
