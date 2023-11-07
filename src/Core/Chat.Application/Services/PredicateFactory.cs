using System.Linq.Expressions;
using System.Reflection;
using Chat.Domain.Entities;
using LinqKit;

namespace Chat.Application.Services;

public sealed class PredicateFactory
{
    public Expression<Func<T, bool>> CreateSearchPredicate<T>(string searchValues)
    {
        var words = searchValues.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var predicate = PredicateBuilder.New<T>(true);

        foreach (var property in GetProperties<T>())
        {
            foreach (var word in words)
            {
                predicate = predicate.Or(entity => property.GetValue(entity).ToString().Contains(word));
            }
        }

        return predicate;
    }
    
    public Expression<Func<User, bool>> CreateSearchPredicate(string searchValues)
    {
        var words = searchValues.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var predicate = PredicateBuilder.New<User>(true);

        foreach (var word in words)
        {
            predicate = predicate.Or(x => x.UserName!.Contains(word))
                                 .Or(x => x.Email.Contains(word))
                                 .Or(x => x.FirstName.Contains(word))
                                 .Or(x => x.LastName.Contains(word))
                                 .Or(x => x.PhoneNumber.Contains(word));
        }

        return predicate;
    }
    
    private static PropertyInfo[] GetProperties<T>()
    {
        return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
}