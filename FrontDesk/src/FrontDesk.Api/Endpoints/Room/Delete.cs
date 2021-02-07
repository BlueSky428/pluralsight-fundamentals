﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models.Room;
using FrontDesk.Core.Aggregates;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace FrontDesk.Api.RoomEndpoints
{
  public class Delete : BaseAsyncEndpoint<DeleteRoomRequest, DeleteRoomResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Delete(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpDelete("api/rooms/{id}")]
    [SwaggerOperation(
        Summary = "Deletes a Room",
        Description = "Deletes a Room",
        OperationId = "rooms.delete",
        Tags = new[] { "RoomEndpoints" })
    ]
    public override async Task<ActionResult<DeleteRoomResponse>> HandleAsync([FromRoute] DeleteRoomRequest request, CancellationToken cancellationToken)
    {
      var response = new DeleteRoomResponse(request.CorrelationId());

      var toDelete = _mapper.Map<Room>(request);
      await _repository.DeleteAsync<Room, int>(toDelete);

      return Ok(response);
    }
  }
}
