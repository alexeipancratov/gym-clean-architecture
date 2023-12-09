using Ardalis.Result;

namespace GymManagement.Domain.Subscriptions;

public static class SubscriptionErrors
{
    public static readonly ValidationError CannotHaveMoreGymsThanSubscriptionAllows = new()
    {
        Identifier = "subscription.cannotHaveMoreGymsThanSubscriptionAllows",
        ErrorMessage = "Cannot have more gyms than subscription allows"
    };
}