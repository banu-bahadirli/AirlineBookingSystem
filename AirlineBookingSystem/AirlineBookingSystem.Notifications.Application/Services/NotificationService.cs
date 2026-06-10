using AirlineBookingSystem.Notifications.Application.Interfaces;
using AirlineBookingSystem.Notifications.Core.Entities;
using AirlineBookingSystem.Notifications.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Notifications.Application.Services
{
	public class NotificationService : INotificationService
	{
		
		public async Task SendNotificationAsync(Notification notification)
		{
			//Simulate sending notification (e.g., via email, SMS, etc.)
			Console.WriteLine($"Sending notification to {notification.Recipient}: {notification.Message}");
		}
	}
}
