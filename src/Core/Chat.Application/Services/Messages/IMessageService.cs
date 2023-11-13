﻿using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Messages;

public interface IMessageService
{
    Task<MessagesPageDto> SearchMessagesPagedAsync(PagedSearchDto searchData);
}