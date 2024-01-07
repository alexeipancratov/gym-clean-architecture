using Ardalis.Result;
using FluentAssertions;
using GymManagement.Application.Gyms.Commands.CreateGym;
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
        var subscription = await CreateSubscription();

        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(
            subscriptionId: subscription.Id);
        
        // Act
        var createGymResult = await _mediator.Send(createGymCommand);
        
        // Assert
        createGymResult.IsSuccess.Should().BeTrue();
        createGymResult.Value.SubscriptionId.Should().Be(subscription.Id);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(200)]
    public async Task CreateGym_WhenCommandContainsInvalidData_ShouldReturnValidationError(int nameLength)
    {
        // Arrange
        string name = new('a', nameLength);
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand(
            name: name);
        
        // Act
        var createGymResult = await _mediator.Send(createGymCommand);
        
        // Assert
        createGymResult.IsSuccess.Should().BeFalse();
        createGymResult.ValidationErrors.Should().Contain(e => e.Identifier == nameof(CreateGymCommand.Name));
    }

    private async Task<Subscription> CreateSubscription()
    {
        var createSubscriptionCommand = SubscriptionCommandFactory.CreateCreateSubscriptionCommand();
        
        var result = await _mediator.Send(createSubscriptionCommand);
        
        result.IsSuccess.Should().BeTrue();
        
        return result.Value;
    }
}