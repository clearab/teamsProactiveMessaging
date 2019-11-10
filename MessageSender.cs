using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace TeamsProactiveMessaging
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

        public async Task SendOneToOneMessage(ConversationResourceResponse conRef, string message)
        {

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

            ConversationResourceResponse response = await this.conClient.Conversations.CreateConversationAsync(conParams);

            return response;
        }

        public async Task<ConversationResourceResponse> CreateAndSendGroupOrChannelMessage(string channelId, string messageText)
        {
            var message = MessageFactory.Text(messageText);

            ConversationParameters conParams = new ConversationParameters
            {
                ChannelData = new TeamsChannelData
                {
                    Channel = new ChannelInfo(channelId)
                },
                Activity = message
            };

            ConversationResourceResponse response = await this.conClient.Conversations.CreateConversationAsync(conParams);

            return response;
        }
    }
}
