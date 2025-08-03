using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewAnalyzer.Application.TenantFeature;
using CodeReviewAnalyzer.Database.ResultSets;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using System.Text.Json;

namespace CodeReviewAnalyzer.Database.Repositories;

internal class TenantRepository(IDatabaseFacade database) : ITenantRepository
{
    private readonly IDatabaseFacade _database = database;

    public async Task<Guid> CreateAsync(Tenant tenant)
    {
        await _database.ExecuteAsync(
            """
                INSERT INTO public."Tenants"
                ("sharedKey", "name", active)
                VALUES(@SharedKey, @Name, @Active);
            
            """,
            new
            {
                SharedKey = tenant.Id,
                tenant.Name,
                tenant.Active,
            });

        return tenant.Id;
    }

    public async Task<Tenant> GetByIdAsync(Guid tenantId)
    {
        const string SelectTenantId =
            """
                SELECT "sharedKey"
                     , "name"
                     , active
                FROM public."Tenants"
                where "sharedKey" = @tenantId;

            """;

        const string SelectDataSources =
            """
                select ds."name" as Name
                    , ds.active as Active
                    , ds."integrationType"
                    -- AzureDevops
                    , ad."devopsUrl" 
                    , ad.pat 
                    , ad.projects 
                    , ad.areas 
                from "DataSource" ds 
                    join "Tenants" t on t.id  = ds.tenants_id 
                    left join "AzureDevops" ad on ad.id  = ds.id 
                where t."sharedKey" = @tenantId;

            """;

        using var resultSets = await _database.QueryMultipleAsync(
            SelectTenantId + SelectDataSources,
            new { tenantId });

        var tenantView = await resultSets.ReadFirstOrDefaultAsync<TenantResultSet>();
        if (tenantView is null)
        {
            return null;
        }

        var dataSourceResultSet = await resultSets.ReadAsync<DataSourceResultSet>();

        var dataSources = new List<DataSource>();

        foreach (var dataSource in dataSourceResultSet)
        {
            switch (dataSource.IntegrationType)
            {
                case IntegrationType.AzureDevops:
                    dataSources.Add(new AzureDevOps(
                        dataSource.Name,
                        new Uri(dataSource.DevOpsUrl),
                        dataSource.Pat,
                        JsonSerializer.Deserialize<string[]>(dataSource.Projects) ?? [],
                        JsonSerializer.Deserialize<string[]>(dataSource.Areas) ?? [],
                        dataSource.Active));
                    break;
            }
        }

        return new Tenant(
            id: tenantId,
            name: tenantView.Name,
            dataSource: dataSources,
            active: tenantView.Active);
    }

    public async Task<Tenant> UpdateAsync(Tenant tenant)
    {
        using var transaction = _database.GetDbConnection().BeginTransaction();

        try
        {
            var handler = new TenantUpdateHandler(_database, tenant, transaction);
            var tenantUpdated = await handler.UpdateTenantAsync();
            if (!tenantUpdated)
            {
                transaction.Rollback();
                return tenant;
            }

            await handler.UpdateDataSourcesAsync();

            transaction.Commit();
            return tenant;
        }
        catch (Exception)
        {
            transaction.Rollback();
            throw;
        }
    }
}
