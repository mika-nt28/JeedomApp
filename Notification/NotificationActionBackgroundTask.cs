using Notification.Helpers;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;

namespace Notification
{
  public sealed class NotificationActionBackgroundTask : IBackgroundTask
  {
    public void Run(IBackgroundTaskInstance taskInstance)
    {
      var details = taskInstance.TriggerDetails 
      as ToastNotificationActionTriggerDetail;

      if (details != null)
      {
        string arguments = details.Argument;

        // process the action
      }
    }
  }
}
