using Chat.Application.Extensions;
using Chat.Application.Mappings;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities.Conversations;
using Chat.Domain.Web;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Conversations;

public sealed class ConversationService : IConversationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ConversationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ConversationsPageDto> SearchUsersPagedAsync(PagedSearchDto searchData)
    {
        var repository = _unitOfWork.GetRepository<Conversation, int>();
        var foundConversations = repository.SearchWhere<ConversationDto>(searchData.SearchFilter);
        var conversationsCount = foundConversations.Count();
        var pageSize = PageInfo.DefaultPageSize;
        var pageInfo = new PageInfo(conversationsCount, searchData.Page);
        var foundConversationsPage = foundConversations
                                     .ToSortedPage(searchData.SortingProperty, searchData.SortingOrder, searchData.Page, pageSize)
                                     .Select(x => x.MapToDto());
        
        return await Task.FromResult(new ConversationsPageDto
        {
            PageInfo = pageInfo,
            Conversations = foundConversationsPage.ToArray()
        });
    }
}