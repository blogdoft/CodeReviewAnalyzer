using CodeReviewInsight.Domain.Features.Configurations.Validators;
using FluentValidation;

namespace CodeReviewInsight.Domain.Features.Configurations.Entities;

public class Tenant : BaseEntity<Tenant>
{
    private readonly List<DataSource> _dataSource = [];

    public Tenant(Guid id, string name, IEnumerable<DataSource> dataSource, bool active = true)
    {
        Id = id;
        Name = name;
        _dataSource.AddRange(dataSource);
        Active = active;
    }

    private bool _active = true;

    public Guid Id { get; init; }

    public string Name { get; init; }

    public bool Active
    {
        get { return _active; }
        init { _active = value; }
    }

    public IEnumerable<DataSource> DataSource { get => _dataSource; }

    public void Deactivate() =>
        _active = false;

    public void AddDataSource(DataSource dataSource) =>
        _dataSource.Add(dataSource);

    public void AddDataSource(IEnumerable<DataSource> dataSource) =>
        _dataSource.AddRange(dataSource);


    public DataSource RemoveDataSource(DataSource dataSource)
    {
        _dataSource.Remove(dataSource);
        return dataSource;
    }

    protected override AbstractValidator<Tenant> GetValidatorInstance() =>
        new TenantValidator();
}
