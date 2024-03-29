using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Gyms;
using Throw;

namespace GymManagement.Domain.Subscriptions;

public class Subscription
{
    public Guid AdminId { get; }

    private readonly int _maxGyms;
    
    private readonly List<Guid> _gymIds = new();
    
    // EF Core requires a private setter, i.e., we can't omit the setter.
    public Guid Id { get; private set; }

    public SubscriptionType SubscriptionType { get; private set; } = null!;

    public Subscription(SubscriptionType subscriptionType, Guid adminId, Guid? id = null)
    {
        SubscriptionType = subscriptionType;
        AdminId = adminId;
        Id = id ?? Guid.NewGuid();
        
        _maxGyms = GetMaxGyms();
    }

    public int GetMaxGyms() => SubscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 1,
        nameof(SubscriptionType.Pro) => 3,
        _ => throw new InvalidOperationException()
    };

    // EF Core requires a private constructor.
    private Subscription()
    {
    }
    
    public UnitResult<OperationError> AddGym(Gym gym)
    {
        // It's not a business rule in our case. Just an unexpected behavior.
        // I.e., it's not something that user provides as input.
        _gymIds.Throw().IfContains(gym.Id);
        
        if (_gymIds.Count >= _maxGyms)
        {
            return UnitResult.Failure(SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows);
        }
        
        _gymIds.Add(gym.Id);

        return UnitResult.Success<OperationError>();
    }
    
    public int GetMaxDailySessions() => SubscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 4,
        nameof(SubscriptionType.Starter) => int.MaxValue,
        nameof(SubscriptionType.Pro) => int.MaxValue,
        _ => throw new InvalidOperationException()
    };
}