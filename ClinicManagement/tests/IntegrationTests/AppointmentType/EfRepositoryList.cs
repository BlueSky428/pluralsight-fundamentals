﻿using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.AppointmentType
{
  public class EfRepositoryList : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryList()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task ListsAppointmentTypeAfterAddingIt()
    {
      await AddAppointmentType();

      var clients = (await _repository.ListAsync<ClinicManagement.Core.Aggregates.AppointmentType, int>()).ToList();

      Assert.True(clients?.Count > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.AppointmentType> AddAppointmentType()
    {
      var client = new AppointmentTypeBuilder().Id(7).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.AppointmentType, int>(client);

      return client;
    }
  }
}
