// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.BotBuilderSamples
{
    public class ProactiveMessagingBot : TeamsActivityHandler
    {
        string _id;
        string _password;

        public ProactiveMessagingBot(IConfiguration config)
        {
            _id = config["MicrosoftAppId"];
            _password = config["MicrosoftAppPassword"];
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            string tenantId = turnContext.Activity.ChannelData.tenant.id;
            var channelId = turnContext.Activity.TeamsGetChannelId();
            MessageSender messageSender = new MessageSender(turnContext.Activity.ServiceUrl, _id, _password);
            turnContext.Activity.RemoveRecipientMention();

            switch (turnContext.Activity.Text)
            {
                case "personal":
                    ConversationResourceResponse oneToOne = await messageSender.CreateOneToOneConversation(turnContext.Activity.From.Id, tenantId);
                    await turnContext.SendActivityAsync(MessageFactory.Text($"The conversation Id is {oneToOne.Id}"));

                    ResourceResponse oneToOneMessageResponse = await messageSender.SendOneToOneMessage(oneToOne.Id, MessageFactory.Text("Hi from Proactive Messaging bot."));
                    await turnContext.SendActivityAsync(MessageFactory.Text($"The message Id is {oneToOneMessageResponse.Id}"));

                    break;

                case "channel" when turnContext.Activity.Conversation.ConversationType.Equals("channel"):
                    ConversationResourceResponse channelThread = await messageSender.CreateAndSendChannelMessage(channelId, MessageFactory.Text("This is a new conversation thread"));
                    await turnContext.SendActivityAsync(MessageFactory.Text($"The thread Id is {channelThread.Id}"));

                    ResourceResponse replyResponse = await messageSender.SendReplyToConversationThread(channelThread.Id, MessageFactory.Text("This is a reply"));
                    ResourceResponse secondReplyResponse = await messageSender.SendReplyToConversationThread(channelThread.Id, MessageFactory.Text("This is the second reply"));

                    break;

                case "channel":
                    await turnContext.SendActivityAsync(MessageFactory.Text("This function only works from a channel."));

                    break;
                default:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Type 'personal' or 'channel' for a proactive message."));
                    break;
            }

        }
    }
}
