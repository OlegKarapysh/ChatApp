﻿using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.Services.Conversations;

public interface IConversationService
{
    Task<IList<int>> GetUserConversationIdsAsync(int userId);
    Task<IList<ConversationDto>> GetAllUserConversationsAsync(int userId);
    Task<ConversationsPageDto> SearchConversationsPagedAsync(PagedSearchDto searchData);
    Task<DialogDto> CreateOrGetDialogAsync(NewDialogDto newDialogData);
    Task<ConversationDto> CreateOrGetGroupChatAsync(NewGroupChatDto newGroupChatData);
    Task<ConversationDto> AddGroupMemberAsync(NewGroupMemberDto groupMemberData);
}