using CodeReviewInsight.Application.Repositories;

namespace CodeReviewInsight.Application.Tests.Services.Processors;

public class DataSourceProcessorTest
{
    public DataSourceProcessorTest()
    {
        PersonRepository = Substitute.For<IPersonRepository>();
    }

    internal IPersonRepository PersonRepository { get; }

    [Fact]
    public void Should_ExtractUsersFromDataSource()
    {
        // Given

        // When

        // Then
        Assert.True(true);
    }
}
