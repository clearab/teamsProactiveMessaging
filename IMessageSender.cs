using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace TeamsProactiveMessaging
{
    public interface IMessageSender
    {
        Task<ConversationResourceResponse> CreateAndSendGroupOrChannelMessage(string channelId, string messageText);
        Task<ConversationResourceResponse> CreateOneToOneConversation(string userId, string tenantId);
        Task SendOneToOneMessage(ConversationResourceResponse conRef, string message);
    }
}