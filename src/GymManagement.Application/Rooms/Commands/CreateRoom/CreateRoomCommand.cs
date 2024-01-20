using CSharpFunctionalExtensions;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Rooms;
using MediatR;

namespace GymManagement.Application.Rooms.Commands.CreateRoom;

public record CreateRoomCommand(Guid GymId, string RoomName) : IRequest<Result<Room, OperationError>>;