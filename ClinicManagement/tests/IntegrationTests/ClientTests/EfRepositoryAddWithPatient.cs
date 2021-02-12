﻿using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;
using ClinicManagement.Core.Aggregates;
using ClinicManagement.Core.Specifications;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryAddWithPatient : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryAddWithPatient()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task AddsPatientAndSetsId()
    {
      var client = await AddClient();
      var patient = client.Patients.First();

      var clientWithPatientsSpec = new ClientByIdIncludePatientsSpecification(client.Id);
      var clientFromDb = (await _repository.ListAsync<ClinicManagement.Core.Aggregates.Client, int>(clientWithPatientsSpec)).First();
      var newPatient = clientFromDb.Patients.First();

      Assert.Equal(patient, newPatient);
      Assert.True(newPatient?.Id > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.Client> AddClient()
    {
      var client = new ClientBuilder().Id(2).Build();
      var patient = new PatientBuilder().Id(3).Build();
      client.Patients.Add(patient);

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
