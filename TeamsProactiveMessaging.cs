// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;

namespace TeamsProactiveMessaging
{
    public class EmptyBot : TeamsActivityHandler
    {
        string _id;
        string _password;

        public EmptyBot(IConfiguration config)
        {
            _id = config["MicrosoftAppId"];
            _password = config["MicrosoftAppPassword"];
        }

        protected override async Task OnTeamsMembersAddedAsync(IList<TeamsChannelAccount> teamsMembersAdded, TeamInfo teamInfo, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach(TeamsChannelAccount member in teamsMembersAdded)
            {

            }
            //if bot
              //store conversation reference
            //if user
              //if group conversation
                //stuff
              //if not
                //stuff


        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync(MessageFactory.Text("Hi"));

            MessageSender sendem = new MessageSender(turnContext.Activity.ServiceUrl, _id, _password);

            var channelId = turnContext.Activity.TeamsGetChannelId();

            var conRef = await sendem.CreateAndSendGroupOrChannelMessage(channelId, "banana");

            if (turnContext.Activity.Text.Equals("banana"))
            {
                ConversationResourceResponse x = sendem.CreateOneToOneConversation(turnContext.Activity.From.Id, turnContext.Activity.ChannelData["tenantId"]);

                await turnContext.SendActivityAsync(MessageFactory.Text(x.Id));
            }

        }
    }
}
