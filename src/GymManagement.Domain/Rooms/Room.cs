namespace GymManagement.Domain.Rooms;

public class Room(
    string name,
    Guid gymId,
    int maxDailySessions,
    Guid? id = null)
{
    public Guid Id { get; } = id ?? Guid.NewGuid();

    public string Name { get; } = name;

    public Guid GymId { get; } = gymId;

    public int MaxDailySessions { get; } = maxDailySessions;
}