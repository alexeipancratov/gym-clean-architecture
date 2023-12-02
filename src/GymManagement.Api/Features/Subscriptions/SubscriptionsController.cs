using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
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
        public async Task<ActionResult<SubscriptionResponse>> CreateSubscription(CreateSubscriptionRequest request)
        {
            var result = await _sender.Send(new CreateSubscriptionCommand(request.SubscriptionType.ToString(), request.AdminId));

            return result
                .Map(subscription => new SubscriptionResponse(subscription.Id, request.SubscriptionType))
                .ToActionResult(this);
        }
        
        [HttpGet("{subscriptionId}")]
        public async Task<ActionResult<SubscriptionResponse>> GetSubscription(Guid subscriptionId)
        {
            var result = await _sender.Send(new GetSubscriptionCommand(subscriptionId));

            return result
                .Map(subscription => new SubscriptionResponse(
                    subscription.Id,
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType)))
                .ToActionResult(this);
        }
    }
}