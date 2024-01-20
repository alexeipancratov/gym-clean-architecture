using GymManagement.Core.ErrorHandling;

namespace GymManagement.Domain.Gyms;

public static class GymErrors
{
    public static readonly OperationError CannotHaveMoreRoomsThanSubscriptionAllows
        = OperationError.Invalid(
            "Gym cannot have more rooms than subscription allows",
            "gym.cannotHaveMoreRoomsThanSubscriptionAllows");
}