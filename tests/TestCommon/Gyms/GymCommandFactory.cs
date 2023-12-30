using GymManagement.Application.Gyms.Commands.CreateGym;
using TestCommon.TestConstants;

namespace TestCommon.Gyms;

public class GymCommandFactory
{
    public static CreateGymCommand CreateCreateGymCommand(
        string name = Constants.Gym.Name,
        Guid? subscriptionId = null)
    {
        return new(
            Name: name,
            SubscriptionId: subscriptionId ?? Constants.Gym.Id);
    }
}