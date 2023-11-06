using System.Linq.Expressions;
using System.Reflection;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Users;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User, int> _userRepository;
    private readonly UserManager<User> _userManager;

    public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _userRepository = _unitOfWork.GetRepository<User, int>();
    }

    public async Task<IList<UserDto>> GetAllUsersAsync()
    {
        return (await _userRepository.GetAllAsync()).Select(x => x.MapToDto()).ToList();
    }

    public async Task<UserDto> GetUserByIdAsync(int id)
    {
        return (await TryGetUserByIdAsync(id)).MapToDto();
    }

    public async Task UpdateUserAsync(UserDto userData, int id)
    {
        var user = await TryGetUserByIdAsync(id);
        user.MapFrom(userData);
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public Task<IList<UserDto>> SearchUsersTest(string searchFilter)
    {
        var factory = new SearchPredicateFactory();
        var predicate = factory
            .CreateSearchPredicate<User>(searchFilter).Compile();
        //var predicate = factory.CreateSearchPredicate(searchFilter);
        var users = _userManager.Users.AsExpandable();
        var foundUsers = users
                         .ToList()
                               .Where(x => predicate.Invoke(x))
                               .ToList()
                               .Select(x => x.MapToDto())
                               .ToList();
        // var props = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        // var a = string.Join(' ', props.Select(x => x.Name));
        // foundUsers.First().FirstName = a;
        // var user = _userManager.Users.First();
        // foundUsers.First().PhoneNumber = props.First().GetValue(user).ToString();
        return Task.FromResult((IList<UserDto>)foundUsers);
    }

    private async Task<User> TryGetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("User", "Id");
    }
}