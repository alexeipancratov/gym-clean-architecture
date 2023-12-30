using Ardalis.Result;
using FluentAssertions;
using GymManagement.Application.SubcutaneousTests.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using MediatR;
using TestCommon.Gyms;
using TestCommon.Subscriptions;

namespace GymManagement.Application.SubcutaneousTests.Gyms.Commands;

[Collection(MediatorFactoryCollection.CollectionName)]
public class CreateGymTests(MediatorFactory mediatorFactory)
{
    private readonly IMediator _mediator = mediatorFactory.CreateMediator();
    
    [Fact]
    public async Task CreateGym_WhenValidCommand_ShouldCreateGym()
    {
        // Arrange
        Subscription subscription = await CreateSubscription();

        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(
            subscriptionId: subscription.Id);
        
        // Act
        Result<Gym> createGymResult = await _mediator.Send(createGymCommand);
        
        // Assert
        createGymResult.IsSuccess.Should().BeTrue();
        createGymResult.Value.SubscriptionId.Should().Be(subscription.Id);
    }

    private async Task<Subscription> CreateSubscription()
    {
        var createSubscriptionCommand = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();
        
        var result = await _mediator.Send(createSubscriptionCommand);
        
        result.IsSuccess.Should().BeTrue();
        
        return result.Value;
    }
}