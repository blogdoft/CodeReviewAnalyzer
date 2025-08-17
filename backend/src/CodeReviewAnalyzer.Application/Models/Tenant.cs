namespace CodeReviewAnalyzer.Application.Models;

public class Tenant
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public static Tenant CreateAsLookup(Guid id) => new()
    {
        Id = id,
        Name = string.Empty,
    };
}
