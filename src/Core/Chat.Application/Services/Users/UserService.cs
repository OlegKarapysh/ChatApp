using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Users;

public sealed class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<User, int> _userRepository;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

    private async Task<User> TryGetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException("User", "Id");
    }
}