using Ardalis.Result;
using FluentAssertions;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using TestCommon.Gyms;
using TestCommon.Subscriptions;

namespace GymManagement.Domain.UnitTests.Subscriptions;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldReturnError()
    {
        // Arrange
        Subscription subscription = SubscriptionFactory.CreateSubscription();
        
        List<Gym> gyms = Enumerable.Range(0, subscription.GetMaxGyms() + 1)
            .Select(_ => GymFactory.CreateGym(id: Guid.NewGuid()))
            .ToList();
        
        // Act
        List<Result> addGymResults = gyms.ConvertAll(g => subscription.AddGym(g));

        // Assert
        List<Result> allButLastGymResults = addGymResults[..^1];
        allButLastGymResults.Should().AllSatisfy(r => r.IsSuccess.Should().BeTrue());

        Result lastGymResult = addGymResults.Last();
        lastGymResult.IsSuccess.Should().BeFalse();
        lastGymResult.ValidationErrors.First().Should().Be(SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows);
    }
}