﻿using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Users;

public interface IUsersWebApiService
{
    Task<WebApiResponse<IList<UserDto>>> GetAllUsers();
    Task<WebApiResponse<UserDto>> GetCurrentUserInfoAsync();
    Task<WebApiResponse<PagedUsersDto>> GetSearchedUsersPage(UsersPagedSearchFilterDto searchData);
    Task<ErrorDetailsDto?> UpdateUserInfoAsync(UserDto userData);
}