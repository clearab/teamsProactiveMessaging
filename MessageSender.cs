using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples
{
    public class MessageSender : IMessageSender
    {
        private ConnectorClient conClient;
        private string serviceUrl;

        public MessageSender(string serviceUrl, string id, string password)
        {
            MicrosoftAppCredentials.TrustServiceUrl(serviceUrl);
            conClient = new ConnectorClient(new Uri(serviceUrl), id, password);
        }

        public async Task<ResourceResponse> SendOneToOneMessage(string conversationId, Activity activity)
        {
            return await conClient.Conversations.SendToConversationAsync(conversationId, activity);

        }

        public async Task<ConversationResourceResponse> CreateOneToOneConversation(string userId, string tenantId)
        {
            var members = new List<ChannelAccount>()
            {
                new ChannelAccount
                {
                    Id = userId
                }
            };

            ConversationParameters conParams = new ConversationParameters
            {
                Members = members,
                TenantId = tenantId
            };

            return await this.conClient.Conversations.CreateConversationAsync(conParams);

        }

        public async Task<ConversationResourceResponse> CreateAndSendChannelMessage(string channelId, Activity activity)
        {
            ConversationParameters conParams = new ConversationParameters
            {
                ChannelData = new TeamsChannelData
                {
                    Channel = new ChannelInfo(channelId)
                },
                Activity = activity
            };

            ConversationResourceResponse response = await this.conClient.Conversations.CreateConversationAsync(conParams);

            return response;
        }

        public async Task<ResourceResponse> SendReplyToConversationThread(string threadId, Activity activity)
        {
            return await this.conClient.Conversations.SendToConversationAsync(threadId, activity);
        }
    }
}
