using Ardalis.Result;

namespace GymManagement.Domain.Gyms;

public static class GymErrors
{
    public static readonly ValidationError CannotHaveMoreRoomsThanSubscriptionAllows = new()
    {
        Identifier = "Gym.CannotHaveMoreRoomsThanSubscriptionAllows",
        ErrorMessage = "Gym cannot have more rooms than subscription allows."
    };
}