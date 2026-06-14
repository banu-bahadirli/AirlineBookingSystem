using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.BuildingBlocks.Common
{
	public class EventBusConstant
	{
		public const string FlightBookedQueue = "flight-booked-queue";
		public const string PaymentProcessQueue = "payment-process-queue";
		public const string NotificationSentQueue = "notification-sent-queue";
	}
}
