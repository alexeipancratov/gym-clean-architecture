using CSharpFunctionalExtensions;
using GymManagement.Api.Shared;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Commands.DeleteSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using GymManagement.Contracts.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;

namespace GymManagement.Api.Features.Subscriptions
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionsController(ISender sender) : ApiController
    {
        private readonly ISender _sender = sender;

        [HttpPost]
        public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
        {
            // API may change, so we need to validate the request.
            if (!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(), out var subscriptionType))
            {
                return Problem("Invalid subscription type");
            }

            var result = await _sender.Send(new CreateSubscriptionCommand(subscriptionType, request.AdminId));

            return result
                .Match(
                    subscription => CreatedAtAction(
                        nameof(CreateSubscription),
                        new { subscriptionId = subscription.Id },
                        new SubscriptionResponse(
                            subscription.Id,
                            request.SubscriptionType)),
                    Problem);
        }
        
        [HttpGet("{subscriptionId}")]
        public async Task<IActionResult> GetSubscription(Guid subscriptionId)
        {
            var result = await _sender.Send(new GetSubscriptionCommand(subscriptionId));

            return result
                .Match(
                    subscription => Ok(
                        new SubscriptionResponse(
                            subscription.Id,
                            Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name))),
                    Problem);
        }

        [HttpDelete("{subscriptionId}")]
        public Task<IActionResult> DeleteSubscription(Guid subscriptionId)
        {
            var result = _sender.Send(new DeleteSubscriptionCommand(subscriptionId));
            
            return result
                .Match(
                    NoContent,
                    Problem);
        }
    }
}