namespace Chat.WebUI.Extensions;

public static class QueryStringParameterExtensions
{
    public static void SetParametersFromQueryString<T>(this T component, NavigationManager navigationManager)
        where T : ComponentBase
    {
        if (!Uri.TryCreate(navigationManager.Uri, UriKind.RelativeOrAbsolute, out var uri))
        {
            throw new InvalidOperationException("Current URI is not valid: " + navigationManager.Uri);
        }

        var queryString = QueryHelpers.ParseQuery(uri.Query);
        foreach (var property in GetProperties<T>())
        {
            var parameterName = GetQueryStringParameterName(property);
            if (parameterName is null)
            {
                continue;
            }

            if (queryString.TryGetValue(parameterName, out var value))
            {
                var convertedValue = ConvertValue(value, property.PropertyType);
                property.SetValue(component, convertedValue);
            }
        }
    }

    public static async Task UpdateQueryString<T>(this T component, NavigationManager navigationManager, IJSRuntime js)
        where T : ComponentBase
    {
        if (!Uri.TryCreate(navigationManager.Uri, UriKind.RelativeOrAbsolute, out var uri))
        {
            throw new InvalidOperationException("Current URI is not valid: " + navigationManager.Uri);
        }

        var parameters = QueryHelpers.ParseQuery(uri.Query);
        foreach (var property in GetProperties<T>())
        {
            var parameterName = GetQueryStringParameterName(property);
            if (parameterName is null)
            {
                continue;
            }
            var value = property.GetValue(component);
            if (value is null)
            {
                parameters.Remove(parameterName);
            }
            else
            {
                var convertedValue = ConvertToString(value);
                parameters[parameterName] = convertedValue;
            }
        }

        var newUri = uri.GetComponents(
            UriComponents.Scheme | UriComponents.Host | UriComponents.Port | UriComponents.Path, UriFormat.UriEscaped);
        foreach (var parameter in parameters)
        {
            foreach (var value in parameter.Value)
            {
                newUri = QueryHelpers.AddQueryString(newUri, parameter.Key, value);
            }
        }

        await js.InvokeVoidAsync("window.history.replaceState", null, "", newUri);
        //navigationManager.NavigateTo(newUri);
    }

    private static object? ConvertValue(StringValues value, Type type)
    {
        return Convert.ChangeType(value[0], type, CultureInfo.InvariantCulture);
    }

    private static string? ConvertToString(object value) => Convert.ToString(value, CultureInfo.InvariantCulture);

    private static PropertyInfo[] GetProperties<T>()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    private static string? GetQueryStringParameterName(PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<QueryStringParameterAttribute>();
        if (attribute is null)
        {
            return null;
        }

        return attribute.Name ?? property.Name;
    }
}