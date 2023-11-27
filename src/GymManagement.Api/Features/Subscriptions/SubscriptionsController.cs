using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Subscriptions
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISender _sender;

        public SubscriptionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {
            var subscriptionId = await _sender.Send(new CreateSubscriptionCommand(request.SubscriptionType.ToString(), request.AdminId));

            var response = new SubscriptionResponse(subscriptionId, request.SubscriptionType);

            return Ok(response);
        }
    }
}