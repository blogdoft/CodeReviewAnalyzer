namespace CodeReviewInsight.Domain.Features.Configurations;

using System;

public readonly struct TenantId : IEquatable<TenantId>
{
    public TenantId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("TenantId cannot be empty.", nameof(value));
        }

        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(TenantId id) => id.Value;

    public static implicit operator TenantId(Guid value) => new TenantId(value);

    public static bool operator ==(TenantId left, TenantId right) => left.Equals(right);

    public static bool operator !=(TenantId left, TenantId right) => !(left == right);

    public static TenantId New() => new TenantId(Guid.NewGuid());

    public bool Equals(TenantId other) => Value.Equals(other.Value);

    public override bool Equals(object? obj) => obj is TenantId other && Equals(other);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();
}
