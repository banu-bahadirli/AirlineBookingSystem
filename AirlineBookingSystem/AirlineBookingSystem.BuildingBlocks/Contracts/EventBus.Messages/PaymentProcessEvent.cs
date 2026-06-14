using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.BuildingBlocks.Contracts.EventBus.Messages
{
	public record PaymentProcessEvent(Guid PaymentId,Guid BookingId,decimal Amount,DateTime PaymentDate);

}
