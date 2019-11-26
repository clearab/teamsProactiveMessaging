using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace TeamsProactiveMessaging
{
    public interface IMessageSender
    {
        Task<ConversationResourceResponse> CreateAndSendChannelMessage(string channelId, Activity activity);
        Task<ConversationResourceResponse> CreateOneToOneConversation(string userId, string tenantId);
        Task<ResourceResponse> SendOneToOneMessage(string conversationId, Activity activity);
        Task<ResourceResponse> SendReplyToConversationThread(string threadId, Activity activity);
    }
}