﻿using System.Collections.Generic;
using System.Linq;

namespace EncodedComparer.Shared.Notifications
{
    public abstract class Notifiable
    {
        private readonly List<Notification> _notifications;

        protected Notifiable() => _notifications = new List<Notification>();

        public IReadOnlyCollection<Notification> Notifications => new List<Notification>(_notifications);

        public void AddNotification(string property, string message)
        {
            _notifications.Add(new Notification(property, message));
        }

        public void AddNotifications(IReadOnlyCollection<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public bool IsValid => !_notifications.Any();
    }
}