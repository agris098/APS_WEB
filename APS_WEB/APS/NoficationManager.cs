using System;
using APS.Models;
using System.Globalization;
using APS.Resources;

namespace APS
{
    public class NoficationManager
    {
        private CultureInfo _culture;

        public NoficationManager(string culture) {
            _culture = new CultureInfo(culture);
        }
        public NotificationModel ClassifiedRejected(string userId ) {
             var n = Notification(
                userId
                , GetString("test")
                , GetString("test")
                );
            return n;
        }
        public NotificationModel ClassifiedExpired(string userId)
        {
            return Notification(
                userId
                , GetString("test")
                , GetString("test")
                );
        }
        private NotificationModel Notification(string userId, string title, string Description)
        {
            return new NotificationModel()
            { 
                UserId = userId,
                Title = title,
                Description = Description,
                Active = true,
                DateTime = DateTime.Now
            };
        }
        // Get resource value From Notification Resource collection
        private string GetString(string key) {
            return Notifications.ResourceManager.GetString(key, _culture);
        }
    }
}