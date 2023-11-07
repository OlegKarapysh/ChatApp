using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using Chat.Domain.Enums;

namespace Chat.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> OrderBy<TEntity>(
        this IQueryable<TEntity> entities, string sortingPropertyName, SortingOrder sortingOrder)
    {
        var sortingMethod = sortingOrder == SortingOrder.Ascending ? "OrderBy" : "OrderByDescending";
        var sortingProperty = typeof(TEntity).GetProperty(sortingPropertyName);
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

        foreach (var property in GetStringProperties<TEntity, TSearch>())
        {
            foreach (var word in words)
            {
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var entity = Expression.Parameter(typeof(TEntity), "entity");
                var propertyAccessor = Expression.PropertyOrField(entity, property.Name);
                var searchWordConstant = Expression.Constant(word);
                var containsCall = Expression.Call(propertyAccessor, containsMethod, searchWordConstant);
                var lambdaPredicate = Expression.Lambda<Func<TEntity, bool>>(containsCall, entity);
                predicate = predicate.Or(lambdaPredicate);
            }
        }

        return entities.AsExpandable().Where(x => ((Expression<Func<TEntity, bool>>)predicate).Invoke(x));
    }
    
    private static IEnumerable<PropertyInfo> GetStringProperties<T>(string[] propertyNames)
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(x => x.PropertyType == typeof(string) && propertyNames.Contains(x.Name));
    }

    private static IEnumerable<PropertyInfo> GetStringProperties<T, U>()
    {
        var targetProperties = typeof(U).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                        .Where(x => x.PropertyType == typeof(string))
                                        .Select(x => x.Name)
                                        .ToArray();
        return GetStringProperties<T>(targetProperties);
    }
}