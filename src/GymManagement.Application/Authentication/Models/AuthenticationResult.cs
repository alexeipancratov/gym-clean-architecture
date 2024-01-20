using GymManagement.Domain.Users;

namespace GymManagement.Application.Authentication.Models;

public record AuthenticationResult(
    User User,
    string Token);