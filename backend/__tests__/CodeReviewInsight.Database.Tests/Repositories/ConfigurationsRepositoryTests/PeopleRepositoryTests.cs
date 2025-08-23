using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Database.Repositories;
using CodeReviewInsight.Database.TablesViews;
using CodeReviewInsight.Domain.Features.Configurations;
using System.Data;
using Person = CodeReviewInsight.Domain.Features.People.Person;

namespace CodeReviewInsight.Database.Tests.Repositories.ConfigurationsRepositoryTests;

public sealed class PeopleRepositoryTests
{
    private readonly IDatabaseFacade _db = Substitute.For<IDatabaseFacade>();
    private readonly PeopleRepository _sut;

    private readonly Faker _faker = new("pt_BR");

    public PeopleRepositoryTests()
    {
        _sut = new PeopleRepository(_db);
    }

    [Fact]
    public async Task Should_InsertAndReturnPerson_When_UpsertPersonSucceedsAsync()
    {
        // Given
        TenantId tenant = Guid.NewGuid();
        var person = FakePerson(withExternalId: true);
        var returnedTable = FakePersonTableFrom(person, tenant.Value);
        _db
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(1));
        _db
            .QuerySingleOrDefaultAsync<PersonTable>(
                Arg.Is<string>(sql => sql.Contains("where p.shared_key", StringComparison.OrdinalIgnoreCase)),
                Arg.Any<object>())
            .Returns(returnedTable);

        // When
        var result = await _sut.UpsertPersonAsync(tenant, person);

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(person.Id);
        result.Name.ShouldBe(person.Name);

        await _db.Received(1).ExecuteAsync(
            Arg.Is<string>(sql => sql.Contains("INSERT INTO public.people", StringComparison.OrdinalIgnoreCase)),
            Arg.Do<object>(paramObject =>
            {
                GetAnonProp<Guid>(paramObject, "TenantId").ShouldBe(tenant.Value);
                GetAnonProp<string>(paramObject, "ExternalId").ShouldBe(person.ExternalId);
                GetAnonProp<Guid>(paramObject, "SharedKey").ShouldBe(person.Id);
                GetAnonProp<string>(paramObject, "Name").ShouldBe(person.Name);
                GetAnonProp<string>(paramObject, "NameSh").ShouldNotBeNull();
            }));
    }

    [Fact]
    public async Task Should_ThrowDataException_When_UpsertPersonNotFoundAfterUpsertAsync()
    {
        // Given
        var tenant = new TenantId(Guid.NewGuid());
        var person = FakePerson(withExternalId: false); // força caminho que usa Id como ExternalId
        _db
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(1));
        _db
            .QuerySingleOrDefaultAsync<PersonTable>(Arg.Any<string>(), Arg.Any<object>())
            .Returns((PersonTable?)null);

        // When
        var ex = await Should.ThrowAsync<DataException>(() => _sut.UpsertPersonAsync(tenant, person));

        // Then
        ex.Message.ShouldContain("Error while storing person");
        await _db.Received(1).ExecuteAsync(Arg.Any<string>(), Arg.Any<object>());
        await _db.Received(1).QuerySingleOrDefaultAsync<PersonTable>(Arg.Any<string>(), Arg.Any<object>());
    }

    [Fact]
    public async Task Should_DelegateToUpsert_When_UpdatePersonIsCalledAsync()
    {
        // Given
        var tenant = new TenantId(Guid.NewGuid());
        var person = FakePerson();

        _db
            .ExecuteAsync(Arg.Any<string>(), Arg.Any<object>())
            .Returns(Task.FromResult(1));
        _db
            .QuerySingleOrDefaultAsync<PersonTable>(Arg.Any<string>(), Arg.Any<object>())
            .Returns(FakePersonTableFrom(person, tenant.Value));

        // When
        var result = await _sut.UpdatePerson(tenant, person);

        // Then
        result.Id.ShouldBe(person.Id);
        await _db.Received(1).ExecuteAsync(Arg.Any<string>(), Arg.Any<object>());
        await _db.Received(1).QuerySingleOrDefaultAsync<PersonTable>(Arg.Any<string>(), Arg.Any<object>());
    }

    [Fact]
    public async Task Should_ReturnNull_When_GetByIdNotFoundAsync()
    {
        // Given
        var tenant = new TenantId(Guid.NewGuid());
        var personId = Guid.NewGuid();

        _db.QuerySingleOrDefaultAsync<PersonTable>(Arg.Any<string>(), Arg.Any<object>())
           .Returns((PersonTable?)null);

        // When
        var result = await _sut.GetByIdAsync(tenant, personId);

        // Then
        result.ShouldBeNull();
        await _db.Received(1).QuerySingleOrDefaultAsync<PersonTable>(
            Arg.Is<string>(sql => sql.Contains("where p.shared_key", StringComparison.OrdinalIgnoreCase)),
            Arg.Is<object>(o =>
                GetAnonProp<Guid>(o, "SharedKey") == personId &&
                GetAnonProp<Guid>(o, "TenantId") == tenant.Value));
    }

    [Fact]
    public async Task Should_ReturnMappedPerson_When_GetByIdFoundAsync()
    {
        // Given
        var tenant = new TenantId(Guid.NewGuid());
        var p = FakePerson();
        var table = FakePersonTableFrom(p, tenant.Value);

        _db.QuerySingleOrDefaultAsync<PersonTable>(Arg.Any<string>(), Arg.Any<object>())
           .Returns(table);

        // When
        var result = await _sut.GetByIdAsync(tenant, p.Id);

        // Then
        result.ShouldNotBeNull();
        result!.Id.ShouldBe(p.Id);
        result.Name.ShouldBe(p.Name);
    }

    [Fact]
    public async Task Should_ReturnPageResultAndForwardParams_When_GetAllAsyncIsCalledAsync()
    {
        // Given
        var tenantId = Guid.NewGuid();
        var searchName = _faker.Name.FirstName();
        var status = (bool?)null; // parâmetro existe mas não é usado no WHERE atual
        var page = new PageFilter() { Page = 2, Size = 10, Order = "name" };

        var people = new List<Person>
        {
            FakePerson(),
            FakePerson()
        };

        _db.QuerySingleOrDefaultAsync<int>(Arg.Any<string>(), Arg.Any<object>())
           .Returns(42);

        _db.QueryAsync<Person>(Arg.Any<string>(), Arg.Any<object>())
           .Returns(people);

        // When
        var result = await _sut.GetAllAsync(tenantId, searchName, status, page);

        // Then
        result.ShouldNotBeNull();
        result.TotalItem.ShouldBe(42);
        result.Data.ShouldBe(people);

        // Verifica que os parâmetros passaram com wildcard do nome
        await _db.Received(1).QuerySingleOrDefaultAsync<int>(
            Arg.Any<string>(),
            Arg.Do<object>(o =>
            {
                GetAnonProp<Guid>(o, "tenantId").ShouldBe(tenantId);

                var nameParam = GetAnonProp<string>(o, "Name");
                nameParam.ShouldNotBeNull();
                nameParam!.StartsWith('%').ShouldBeTrue();
                nameParam.EndsWith('%').ShouldBeTrue();
                nameParam.Contains(searchName, StringComparison.OrdinalIgnoreCase).ShouldBeTrue();
            }));

        await _db.Received(1).QueryAsync<Person>(
            Arg.Any<string>(),
            Arg.Is<object>(o =>
                GetAnonProp<Guid>(o, "tenantId") == tenantId &&
                GetAnonProp<string>(o, "Name")!.Contains(searchName, StringComparison.OrdinalIgnoreCase)));
    }

    #region Helpers

    private static PersonTable FakePersonTableFrom(Person p, Guid tenantSharedKey)
    {
        return new PersonTable
        {
            TenantId = tenantSharedKey,
            ExternalId = p.ExternalId ?? p.Id.ToString(),
            Id = p.Id,
            Name = p.Name,
            AvatarUrl = p.AvatarUrl?.ToString()
        };
    }

    private static T? GetAnonProp<T>(object anon, string propName)
    {
        var prop = anon.GetType().GetProperty(propName);
        return prop is null ? default : (T?)prop.GetValue(anon);
    }

    private Person FakePerson(bool withExternalId = true)
    {
        var id = Guid.NewGuid();
        return new Person()
        {
            Id = id,
            ExternalId = withExternalId ? _faker.Random.Guid().ToString() : null,
            Name = _faker.Person.FullName,
            AvatarUrl = new Uri("https://example.com/avatar.png")
        };
    }

    #endregion
}
