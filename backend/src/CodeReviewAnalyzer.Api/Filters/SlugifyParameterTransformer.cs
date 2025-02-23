using System.Text.RegularExpressions;

namespace CodeReviewAnalyzer.Api.Filters;

public class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object? value)
    {
        if (value == null)
        {
            return string.Empty;
        }

        var input = value?.ToString() ?? string.Empty;

        // Substitui camelCase ou PascalCase por palavras separadas por hífen
        return Regex.Replace(input, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
