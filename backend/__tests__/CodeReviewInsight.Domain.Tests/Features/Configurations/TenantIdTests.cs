using CodeReviewInsight.Domain.Features.Configurations;

namespace CodeReviewInsight.Domain.Tests.Features.Configurations;

public class TenantIdTests
{
    private readonly Faker _faker = new("pt_BR");

    internal interface IGuidSink
    {
        void Accept(Guid id);
        Guid Echo(Guid id);
    }

    [Fact]
    public void Should_ThrowArgumentException_When_ConstructedWithEmptyGuid()
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => new TenantId(Guid.Empty));
    }

    [Fact]
    public void Should_GenerateNonEmptyAndUniqueIds_When_NewIsCalledTwice()
    {
        // Act
        TenantId id1 = TenantId.New();
        TenantId id2 = TenantId.New();

        // Assert
        id1.Value.ShouldNotBe(Guid.Empty);
        id2.Value.ShouldNotBe(Guid.Empty);
        id1.ShouldNotBe(id2);
        (id1 != id2).ShouldBeTrue();
    }

    [Fact]
    public void Should_ReturnUnderlyingGuid_When_ImplicitlyConvertedToGuid()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId tenantId = new TenantId(guid);

        // Act
        Guid asGuid = tenantId;

        // Assert
        asGuid.ShouldBe(guid);
    }

    [Fact]
    public void Should_CreateTenantIdWithSameValue_When_ImplicitlyConvertedFromGuid()
    {
        // Arrange
        var guid = _faker.Random.Guid();

        // Act
        TenantId tenantId = guid;

        // Assert
        tenantId.Value.ShouldBe(guid);
    }

    [Fact]
    public void Should_CallConsumerWithSameGuid_When_PassingTenantIdToGuidParameter_UsingNSubstitute()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId tenantId = new TenantId(guid);
        var sink = Substitute.For<IGuidSink>();

        // Act
        sink.Accept(tenantId);

        // Assert
        sink.Received(1).Accept(guid);
    }

    [Fact]
    public void Should_EchoSameGuid_When_InterfaceReturnsGuid_UsingNSubstitute()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId tenantId = new TenantId(guid);
        var sink = Substitute.For<IGuidSink>();
        sink.Echo(Arg.Any<Guid>()).Returns(ci => (Guid)ci[0]!);

        // Act
        var echoed = sink.Echo(tenantId);

        // Assert
        echoed.ShouldBe(guid);
    }

    [Fact]
    public void Should_BeEqualAndHaveSameHashCode_When_ValuesAreTheSame()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId id1 = new TenantId(guid);
        TenantId id2 = new TenantId(guid);

        // Assert
        id1.Equals(id2).ShouldBeTrue();
        (id1 == id2).ShouldBeTrue();
        (id1 != id2).ShouldBeFalse();
        id1.GetHashCode().ShouldBe(id2.GetHashCode());
    }

    [Fact]
    public void Should_BeDifferent_When_ValuesAreDifferent()
    {
        // Arrange
        TenantId id1 = new TenantId(_faker.Random.Guid());
        TenantId id2 = new TenantId(_faker.Random.Guid());

        // Assert
        id1.Equals(id2).ShouldBeFalse();
        (id1 == id2).ShouldBeFalse();
        (id1 != id2).ShouldBeTrue();
    }

    [Fact]
    public void Should_HandleEqualsObject_When_ObjectIsNullOrDifferentType()
    {
        // Arrange
        TenantId id = new TenantId(_faker.Random.Guid());
        object otherType = "string";

        // Assert
        id.Equals(null).ShouldBeFalse();
        id.Equals(otherType).ShouldBeFalse();
    }

    [Fact]
    public void Should_ReturnGuidString_When_ToStringIsCalled()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId tenantId = new TenantId(guid);

        // Act
        string text = tenantId.ToString();

        // Assert
        text.ShouldBe(guid.ToString());
    }

    [Fact]
    public void Should_ReturnTrue_When_OperatorEqualsUsedOnSameValues()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId id1 = new TenantId(guid);
        TenantId id2 = new TenantId(guid);

        // Act & Assert
        (id1 == id2).ShouldBeTrue();
    }

    [Fact]
    public void Should_ReturnFalse_When_OperatorEqualsUsedOnDifferentValues()
    {
        // Arrange
        TenantId id1 = new TenantId(_faker.Random.Guid());
        TenantId id2 = new TenantId(_faker.Random.Guid());

        // Act & Assert
        (id1 == id2).ShouldBeFalse();
    }

    [Fact]
    public void Should_ReturnTrue_When_OperatorNotEqualsUsedOnDifferentValues()
    {
        // Arrange
        TenantId id1 = new TenantId(_faker.Random.Guid());
        TenantId id2 = new TenantId(_faker.Random.Guid());

        // Act & Assert
        (id1 != id2).ShouldBeTrue();
    }

    [Fact]
    public void Should_ReturnFalse_When_OperatorNotEqualsUsedOnSameValues()
    {
        // Arrange
        var guid = _faker.Random.Guid();
        TenantId id1 = new TenantId(guid);
        TenantId id2 = new TenantId(guid);

        // Act & Assert
        (id1 != id2).ShouldBeFalse();
    }
}
