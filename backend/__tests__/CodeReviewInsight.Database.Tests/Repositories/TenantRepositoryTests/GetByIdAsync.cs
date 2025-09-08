using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Database.ResultSets;
using CodeReviewInsight.Domain.Enums;
using CodeReviewInsight.Domain.Features.Configurations;
using CodeReviewInsight.Domain.Features.Configurations.Entities;
using NSubstitute.ReturnsExtensions;
using System.Text.Json;

namespace CodeReviewInsight.Database.Tests.Repositories.TenantRepositoryTests;

public class GetByIdAsync : BaseTenantRepositoryTests
{

    [Fact]
    public async Task Should_ReturnTenantEntity_When_TenantNotFoundAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var tenantRow = BuildTenantResultSet(tenantId);
        var dataSourceRow = BuildDataSourceResultSet();
        var gridReaderFacade = Substitute.For<IGridReaderFacade>();
        gridReaderFacade
            .ReadFirstOrDefaultAsync<TenantResultSet>()
            .Returns(tenantRow);
        gridReaderFacade
            .ReadAsync<DataSourceResultSet>()
            .Returns(dataSourceRow);
        DatabaseFacade
            .QueryMultipleAsync(Arg.Any<string>(), Arg.Any<object?>())
            .Returns(gridReaderFacade);
        var repository = BuildTenantRepository();

        // When
        var tenantFound = await repository.GetByIdAsync(tenantId);

        // Then
        tenantFound.ShouldNotBeNull();
        tenantFound.ShouldBeEquivalentTo(new Tenant(
            id: tenantId,
            name: tenantRow.Name,
            dataSource: [ new AzureDevOps(
                name: dataSourceRow[0].Name,
                devOpsUrl: new Uri(dataSourceRow[0].DevOpsUrl),
                pat: dataSourceRow[0].Pat,
                projects: JsonSerializer.Deserialize<string[]>(dataSourceRow[0].Projects) ?? [],
                areas: JsonSerializer.Deserialize<string[]>(dataSourceRow[0].Areas) ?? [],
                active: dataSourceRow[0].Active)],
            active: tenantRow.Active));
        gridReaderFacade.Received(1).Dispose();
    }


    [Fact]
    public async Task Should_ReturnNull_When_TenantNotFoundAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var gridReaderFacade = Substitute.For<IGridReaderFacade>();
        gridReaderFacade
            .ReadFirstOrDefaultAsync<TenantResultSet>()
            .ReturnsNull();
        DatabaseFacade
            .QueryMultipleAsync(Arg.Any<string>(), Arg.Any<object?>())
            .Returns(gridReaderFacade);
        var repository = BuildTenantRepository();

        // When
        var tenantFound = await repository.GetByIdAsync(tenantId);

        // Then
        tenantFound.ShouldBeNull();
        await gridReaderFacade.DidNotReceive().ReadAsync<DataSourceResultSet>();
        gridReaderFacade.Received(1).Dispose();
    }

    [Fact]
    public async Task Should_NotMapDataSource_When_IntegrationTypeIsNotSupportedAsync()
    {
        // Given
        TenantId tenantId = Guid.NewGuid();
        var tenantRow = BuildTenantResultSet(tenantId);
        var dataSourceRow = BuildDataSourceResultSet((IntegrationType)999);
        var gridReaderFacade = Substitute.For<IGridReaderFacade>();
        gridReaderFacade
            .ReadFirstOrDefaultAsync<TenantResultSet>()
            .Returns(tenantRow);
        gridReaderFacade
            .ReadAsync<DataSourceResultSet>()
            .Returns(dataSourceRow);
        DatabaseFacade
            .QueryMultipleAsync(Arg.Any<string>(), Arg.Any<object?>())
            .Returns(gridReaderFacade);
        var repository = BuildTenantRepository();

        // When
        var tenantFound = await repository.GetByIdAsync(tenantId);

        // Then
        tenantFound.ShouldNotBeNull();
        tenantFound.DataSource.Count().ShouldBe(0);
    }
}
