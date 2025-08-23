using BlogDoFT.Libs.DapperUtils.Abstractions;
using CodeReviewInsight.Api.Features.People;
using CodeReviewInsight.Api.Features.People.Models.Requests;
using CodeReviewInsight.Api.Features.People.Models.Responses;
using CodeReviewInsight.Api.Models.Paging;
using CodeReviewInsight.Application.Models.PagingModels;
using CodeReviewInsight.Application.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Person = CodeReviewInsight.Domain.Features.People.Person;

namespace CodeReviewInsight.Api.Tests.Features.People;

public sealed class PeopleControllerTests
{
    private readonly IPeople _people = Substitute.For<IPeople>();

    [Fact]
    public async Task Should_ReturnOkWithPeoplePaginated_When_GetAllAsyncIsCalledAsync()
    {
        // Given
        var controller = BuildController();
        var tenantId = Guid.NewGuid();
        const string PersonName = "John D*";
        bool? status = null;
        var pageReq = FakePageRequest();

        var peopleList = new List<Person> { FakePerson(), FakePerson() };
        var repoReturn = new PageReturn<IEnumerable<Person>>(peopleList, totalItem: 42);

        _people
            .GetAllAsync(tenantId, PersonName, status, pageReq.ToPageFilter())
            .Returns(repoReturn);

        // When
        var result = await controller.GetAllAsync(_people, tenantId, PersonName, status, pageReq);

        // Then
        var ok = result as OkObjectResult;
        ok.ShouldNotBeNull();
        ok!.StatusCode.ShouldBe(200);
        ok.Value.ShouldBeOfType<PeoplePaginated>();

        await _people.Received(1).GetAllAsync(
            tenantId,
            PersonName,
            status,
            Arg.Is<PageFilter>(pf =>
                pf.Page == pageReq.Page &&
                pf.Size == pageReq.Size &&
                string.Equals(pf.Order, pageReq.Order, StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public async Task Should_ReturnCreatedWithLocation_When_PostAsyncSucceedsAsync()
    {
        // Given
        var urlHelper = Substitute.For<IUrlHelper>();
        var controller = BuildController(urlHelper);

        var tenantId = Guid.NewGuid();
        var request = new PersonRequest
        {
            // Preencha os campos necessários para ToEntity no seu modelo
            Name = "Jane Smith",
            ExternalId = "ext-123",
            AvatarUri = "https://example.com/jane.png",
        };

        var created = FakePerson();
        _people.UpsertPersonAsync(tenantId, Arg.Any<Person>()).Returns(created);

        // A URL de recurso criada pelo controller
        var expectedLocation = $"/api/{tenantId}/people/{created.Id}";
        urlHelper.Action(Arg.Any<UrlActionContext>()).Returns(expectedLocation);

        // When
        var result = await controller.PostAsync(_people, tenantId, request);

        // Then
        var createdResult = result as CreatedResult;
        createdResult.ShouldNotBeNull();
        createdResult!.StatusCode.ShouldBe(201);
        createdResult.Location.ShouldBe(expectedLocation);

        // O body anônimo contém tenantId e personId
        var body = createdResult.Value;
        body.ShouldNotBeNull();

        // Valida que Upsert foi chamado com o tenant correto
        await _people.Received(1).UpsertPersonAsync(tenantId, Arg.Any<Person>());
    }

    [Fact]
    public async Task Should_ReturnNotFound_When_GetByIdAsyncReturnsNullAsync()
    {
        // Given
        var controller = BuildController();
        var tenantId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        _people.GetByIdAsync(tenantId, personId).Returns((Person?)null);

        // When
        var result = await controller.GetByIdAsync(_people, tenantId, personId);

        // Then
        result.ShouldBeOfType<NotFoundResult>();
        await _people.Received(1).GetByIdAsync(tenantId, personId);
    }

    [Fact]
    public async Task Should_ReturnOkWithPersonResponse_When_GetByIdAsyncSucceedsAsync()
    {
        // Given
        var controller = BuildController();
        var tenantId = Guid.NewGuid();
        var person = FakePerson();

        _people.GetByIdAsync(tenantId, person.Id).Returns(person);

        // When
        var result = await controller.GetByIdAsync(_people, tenantId, person.Id);

        // Then
        var ok = result as OkObjectResult;
        ok.ShouldNotBeNull();
        ok!.StatusCode.ShouldBe(200);
        ok.Value.ShouldBeOfType<PersonResponse>();

        await _people.Received(1).GetByIdAsync(tenantId, person.Id);
    }

    [Fact]
    public async Task Should_ReturnOkWithPersonResponse_When_PutAsyncSucceedsAsync()
    {
        // Given
        var controller = BuildController();
        var tenantId = Guid.NewGuid();
        var personId = Guid.NewGuid();

        var request = new PersonRequest
        {
            Name = "Taylor",
            ExternalId = "ext-xyz",
            AvatarUri = "https://example.com/t.png",
        };

        var updated = FakePerson(personId);
        _people.UpsertPersonAsync(tenantId, Arg.Any<Person>()).Returns(updated);

        // When
        var result = await controller.PutAsync(_people, tenantId, personId, request);

        // Then
        var ok = result as OkObjectResult;
        ok.ShouldNotBeNull();
        ok!.StatusCode.ShouldBe(200);
        ok.Value.ShouldBeOfType<PersonResponse>();

        await _people.Received(1).UpsertPersonAsync(tenantId, Arg.Is<Person>(p => p.Id == personId));
    }

    private static Person FakePerson(Guid? personId = null) => new Person
    {
        Id = personId ?? Guid.NewGuid(),
        ExternalId = Guid.NewGuid().ToString("N"),
        Name = "John Doe",
        AvatarUrl = new Uri("https://example.com/a.png"),
    };

    private static PaginatedRequest FakePageRequest() => new PaginatedRequest
    {
        Page = 2,
        Size = 10,
        Order = "name",
    };

    private static PeopleController BuildController(IUrlHelper? urlHelper = null)
    {
        var controller = new PeopleController();
        if (urlHelper is not null)
        {
            controller.Url = urlHelper;
        }
        return controller;
    }
}
