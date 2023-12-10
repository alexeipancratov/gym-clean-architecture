using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using GymManagement.Application.Rooms.Commands.CreateRoom;
using GymManagement.Contracts.Rooms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Api.Features.Rooms;

[Route("gyms/{gymId:guid}/rooms")]
public class RoomsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;
    
    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<RoomResponse>> CreateRoom(CreateRoomRequest request, Guid gymId)
    {
        var result = await _sender.Send(new CreateRoomCommand(gymId, request.Name));

        return result
            .Map(room => new RoomResponse(room.Id, room.Name));
    }
}