using AirlineBookingSystem.Bookings.Application.Commands;
using AirlineBookingSystem.Bookings.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AirlineBookingSystem.Bookings.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> AddBooking([FromBody] CreateBookingCommand request)
        {
            var result = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetBookingById), new { id = result}, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _mediator.Send(new GetBookingQuery(id));
            return Ok(booking);


        }
    }
}
