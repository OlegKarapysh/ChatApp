﻿using Chat.Domain.DTOs;
using Chat.Domain.DTOs.Groups;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Groups;

public interface IGroupsWebApiService
{
    Task<WebApiResponse<IList<GroupInfoDto>>> GetAllGroupsInfoAsync();
    Task<ErrorDetailsDto?> DeleteGroupAsync(int groupId);
}