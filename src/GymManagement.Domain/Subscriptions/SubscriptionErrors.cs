using GymManagement.Core.ErrorHandling;

namespace GymManagement.Domain.Subscriptions;

public static class SubscriptionErrors
{
    public static readonly OperationError CannotHaveMoreGymsThanSubscriptionAllows
        = OperationError.Invalid(
            "Cannot have more gyms than subscription allows",
            "subscription.cannotHaveMoreGymsThanSubscriptionAllows");
}