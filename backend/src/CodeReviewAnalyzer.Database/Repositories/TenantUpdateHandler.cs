using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Exceptions;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.Data;
using System.Text.Json;

namespace CodeReviewAnalyzer.Database.Repositories;

internal class TenantUpdateHandler
{
    private readonly IDatabaseFacade _database;
    private readonly Tenant _tenant;
    private readonly IDbTransaction _transaction;

    public TenantUpdateHandler(IDatabaseFacade databaseFacade, Tenant tenant, IDbTransaction transaction)
    {
        _database = databaseFacade;
        _tenant = tenant;
        _transaction = transaction;
    }

    public async Task<bool> UpdateTenantAsync()
    {
        var affectedRecords = await _database.ExecuteAsync(
            TenantStmt.UpdateTenant,
            new
            {
                _tenant.Name,
                _tenant.Active,
                _tenant.Id,
            },
            _transaction);
        return affectedRecords == 1;
    }

    public async Task UpdateDataSourcesAsync()
    {
        await _database.ExecuteAsync(
            TenantStmt.DeleteAllDataSources,
            new { _tenant.Id },
            _transaction);

        foreach (var dataSource in _tenant.DataSource)
        {
            var dataSourceId = await _database.ExecuteScalarAsync<int>(
                TenantStmt.DataSourceInsert,
                new
                {
                    SharedKey = _tenant.Id,
                    _tenant.Name,
                    _tenant.Active,
                    IntegrationType = dataSource.GetIntegrationType().ToString(),
                });
            switch (dataSource.GetIntegrationType())
            {
                case IntegrationType.AzureDevops:
                    await UpdateAzureDevOpsAsync(dataSourceId, (AzureDevOps)dataSource);
                    break;
                default:
                    throw new UnsuportedDataSourceTypeException(dataSource.GetIntegrationType());
            }
        }
    }

    private async Task UpdateAzureDevOpsAsync(int dataSourceId, AzureDevOps dataSource)
    {
        await _database.ExecuteAsync(
            TenantStmt.InsertAzureDevOps,
            new
            {
                Id = dataSourceId,
                DevOpsUrl = dataSource.DevOpsUrl.ToString(),
                Pat = dataSource.Pat,
                Projects = JsonSerializer.Serialize(dataSource.Projects),
                Areas = JsonSerializer.Serialize(dataSource.Areas),
            },
            _transaction);
    }
}
