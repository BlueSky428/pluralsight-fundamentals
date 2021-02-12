﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using BlazorShared.Models;
using BlazorShared.Models.Patient;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using PluralsightDdd.SharedKernel.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace ClinicManagement.Api.PatientEndpoints
{
  public class Create : BaseAsyncEndpoint
    .WithRequest<CreatePatientRequest>
    .WithResponse<CreatePatientResponse>
  {
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public Create(IRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [HttpPost("api/patients")]
    [SwaggerOperation(
        Summary = "Creates a new Patient",
        Description = "Creates a new Patient",
        OperationId = "patients.create",
        Tags = new[] { "PatientEndpoints" })
    ]
    public override async Task<ActionResult<CreatePatientResponse>> HandleAsync(CreatePatientRequest request, CancellationToken cancellationToken)
    {
      var response = new CreatePatientResponse(request.CorrelationId());

      var spec = new ClientByIdIncludePatientsSpecification(request.ClientId);
      var client = await _repository.GetAsync<Client, int>(spec);
      if (client == null) return NotFound();

      var newPatient = new Patient(client.Id, request.PatientName, "", new Core.ValueObjects.AnimalType("Dog", "Husky"));
      client.Patients.Add(newPatient);

      await _repository.UpdateAsync<Client, int>(client);

      var dto = _mapper.Map<PatientDto>(newPatient);
      response.Patient = dto;

      return Ok(response);
    }
  }
}
