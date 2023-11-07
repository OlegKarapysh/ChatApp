using System.Linq.Expressions;
using Chat.Domain.Entities;
using Chat.Domain.Enums;

namespace Chat.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<User> OrderBy(this IQueryable<User> users, string propertyName, SortingOrder order)
    {
        return propertyName switch
        {
            nameof(User.UserName) => users.OrderBy(x => x.UserName, order),
            nameof(User.FirstName) => users.OrderBy(x => x.FirstName, order),
            nameof(User.LastName) => users.OrderBy(x => x.LastName, order),
            nameof(User.Email) => users.OrderBy(x => x.Email, order),
            nameof(User.PhoneNumber) => users.OrderBy(x => x.PhoneNumber, order),
            _ => users.OrderBy(x => x.UserName, order),
        };
    }

    private static IQueryable<User> OrderBy(
        this IQueryable<User> users, Expression<Func<User, string?>> sortExpression, SortingOrder order)
    {
        return order == SortingOrder.Ascending
            ? users.OrderBy(sortExpression)
            : users.OrderByDescending(sortExpression);
    }
}