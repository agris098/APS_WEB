using System;
using APS.Models;
using System.Globalization;
using APS.Resources;

namespace APS
{
    public class NoficationManager
    {
        private CultureInfo _culture;

        public NoficationManager() {
            _culture = new CultureInfo("ru");
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
                , "Expired"
                , "EXXXXXXXXXXXXX"
                );
        }
        public NotificationModel ClassifiedMarkedExpired(string userId)
        {
            return Notification(
                userId
                , "Expired"
                , "EXXXXXXXXXXXXX"
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
        private string GetString(string key) {
            return Notifications.ResourceManager.GetString(key, _culture);
        }
    }
}