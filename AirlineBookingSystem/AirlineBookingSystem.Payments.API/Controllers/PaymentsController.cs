using AirlineBookingSystem.Payments.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.Payments.API.Controllers
{

	[ApiController]
	[Route("api/payments")]
	public class FlightsController : ControllerBase
	{
		private readonly IMediator _mediator;

		public FlightsController(IMediator mediator)
		{
			_mediator = mediator;
		}


		[HttpPost]
		public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
		{
			var id = await _mediator.Send(command);
			return CreatedAtAction(nameof(ProcessPayment), new { id }, command);
		}

		[HttpPost("refund/{id}")]
		public async Task<IActionResult> RefundPayment(Guid id)
		{
			await _mediator.Send(new RefundPaymentCommand(id));
			return NoContent();
		}
	}
	}
