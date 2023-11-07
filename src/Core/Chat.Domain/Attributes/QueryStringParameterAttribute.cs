namespace Chat.Domain.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public sealed class QueryStringParameterAttribute : Attribute
{
    public string? Name { get; }

    public QueryStringParameterAttribute(string name)
    {
        Name = name;
    }

    public QueryStringParameterAttribute()
    {
    }
}