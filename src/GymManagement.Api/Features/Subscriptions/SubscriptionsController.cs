using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Features.Subscriptions
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController(ISender sender) : ControllerBase
    {
        private readonly ISender _sender = sender;

        [HttpPost]
        public async Task<ActionResult<SubscriptionResponse>> CreateSubscription(CreateSubscriptionRequest request)
        {
            // API may change, so we need to validate the request.
            if (!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType))
            {
                return Problem("Invalid subscription type");
            }

            var result = await _sender.Send(new CreateSubscriptionCommand(subscriptionType, request.AdminId));

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
                    Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name)))
                .ToActionResult(this);
        }
    }
}