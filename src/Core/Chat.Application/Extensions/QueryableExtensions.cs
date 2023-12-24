

namespace Chat.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> entities, string sortingPropertyName, SortingOrder sortingOrder)
    {
        var sortingProperty = typeof(TEntity).GetProperty(sortingPropertyName);
        if (sortingProperty is null)
        {
            throw new ArgumentException("Property with this name is not found!", nameof(sortingPropertyName));
        }
        
        var sortingMethod = sortingOrder == SortingOrder.Ascending ? "OrderBy" : "OrderByDescending";
        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var propertyAccess = Expression.MakeMemberAccess(parameter, sortingProperty);
        var orderByLambda = Expression.Lambda(propertyAccess, parameter);
        var resultExpression = Expression.Call(
            typeof(Queryable), sortingMethod, new[] { typeof(TEntity), sortingProperty.PropertyType },
            entities.Expression, Expression.Quote(orderByLambda));
        
        return entities.Provider.CreateQuery<TEntity>(resultExpression);
    }

    public static IQueryable<TEntity> SearchWhere<TEntity, TSearch>(
        this IQueryable<TEntity> entities, string? searchValues)
    {
        if (string.IsNullOrEmpty(searchValues))
        {
            return entities;
        }
        
        var words = searchValues.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var predicate = PredicateBuilder.New<TEntity>(true);
        foreach (var property in GetStringableProperties<TEntity, TSearch>())
        {
            foreach (var word in words)
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                var entity = Expression.Parameter(typeof(TEntity), "entity");
                var propertyAccessor = Expression.Property(entity, property);
                var propertyAsObject = Expression.Convert(propertyAccessor, typeof(object));
                var nullCheck = Expression.ReferenceEqual(propertyAsObject, Expression.Constant(null));
                Expression stringifiedProperty = property.PropertyType == typeof(string)
                    ? propertyAccessor
                    : Expression.Call(propertyAccessor, TryGetToStringMethod(property)!);
                var containsCall = Expression.Call(stringifiedProperty, containsMethod, Expression.Constant(word));
                var conditionalExpression = Expression.Condition(nullCheck, Expression.Constant(false), containsCall);
                var lambdaPredicate = Expression.Lambda<Func<TEntity, bool>>(conditionalExpression, entity);
                predicate = predicate.Or(lambdaPredicate);
            }
        }

        return entities.AsExpandable()!.Where(x => ((Expression<Func<TEntity, bool>>)predicate).Invoke(x));
    }
    
    public static IQueryable<T> ToSortedPage<T>(this IQueryable<T> entities,
        string sortingProperty, SortingOrder sortingOrder, int page, int pageSize)
    {
        return entities.OrderBy(sortingProperty, sortingOrder)
                         .Skip((page - 1) * pageSize)
                         .Take(pageSize);
    }

    private static IEnumerable<PropertyInfo> GetStringableProperties<TEntity, TSearch>()
    {
        var targetProperties = typeof(TSearch).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                              .Select(x => x.Name)
                                              .ToArray();
        return typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => targetProperties.Contains(x.Name) && TryGetToStringMethod(x) is not null);
    }

    private static MethodInfo? TryGetToStringMethod(PropertyInfo? propertyInfo)
    {
        return propertyInfo?.PropertyType.GetMethod("ToString", Type.EmptyTypes);
    }
}