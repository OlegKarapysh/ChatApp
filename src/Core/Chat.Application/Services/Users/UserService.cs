using LinqKit;
using Microsoft.AspNetCore.Identity;
using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Application.Extensions;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities;
using Chat.Domain.Web;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Users;

public sealed class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User, int> _userRepository;
    private readonly UserManager<User> _userManager;
    private readonly PredicateFactory _predicateFactory;

    public UserService(IUnitOfWork unitOfWork, UserManager<User> userManager, PredicateFactory predicateFactory)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _predicateFactory = predicateFactory;
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

    public async Task<PagedList<UserDto>> SearchUsersPagedAsync(UsersPagedSearchFilterDto searchData)
    {
        var searchPredicate = _predicateFactory.CreateSearchPredicate(searchData.SearchFilter);
        var foundUsers = string.IsNullOrEmpty(searchData.SearchFilter)
            ? _userManager.Users
            : _userManager.Users.AsExpandable()
                          .Where(x => searchPredicate.Invoke(x));
        var usersCount = foundUsers.Count();
        var pageSize = PagedList<User>.DefaultPageSize;
        var foundUsersPage = foundUsers.Skip((searchData.Page - 1) * pageSize)
                                       .Take(pageSize)
                                       .OrderBy(searchData.SortingProperty, searchData.SortingOrder)
                                       .Select(x => x.MapToDto());
        
        return await Task.FromResult(new PagedList<UserDto>(foundUsersPage, usersCount, searchData.Page));
    }

    private async Task<User> TryGetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("User", "Id");
    }
}