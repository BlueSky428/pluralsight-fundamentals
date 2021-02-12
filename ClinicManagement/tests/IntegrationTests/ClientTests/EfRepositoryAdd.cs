﻿using System.Linq;
using System.Threading.Tasks;
using ClinicManagement.Infrastructure.Data;
using UnitTests.Builders;
using Xunit;

namespace IntegrationTests.ClientTests
{
  public class EfRepositoryAdd : BaseEfRepoTestFixture
  {
    private readonly EfRepository _repository;

    public EfRepositoryAdd()
    {
      _repository = GetRepository();
    }

    [Fact]
    public async Task AddsClientAndSetsId()
    {
      var client = await AddClient();

      var newClient = (await _repository.ListAsync<ClinicManagement.Core.Aggregates.Client, int>()).FirstOrDefault();

      Assert.Equal(client, newClient);
      Assert.True(newClient?.Id > 0);
    }

    private async Task<ClinicManagement.Core.Aggregates.Client> AddClient()
    {
      var client = new ClientBuilder().Id(2).Build();

      await _repository.AddAsync<ClinicManagement.Core.Aggregates.Client, int>(client);

      return client;
    }
  }
}
