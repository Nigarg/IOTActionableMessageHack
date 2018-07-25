using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http.Headers;

using Microsoft.Graph;
using Newtonsoft.Json.Linq;

namespace IOTAmHackCore.Helpers
{
    public class AMHelper
    {
        public static async Task SendActionableMessageAsync(string[] toRecipients, string visitorImage, string visitorName)
        {
            try
            {
                var accessToken = string.Empty; //Get Access token for owner;

                var graphClient = new GraphServiceClient(
                    new DelegateAuthenticationProvider(
                        (requestMessage) => 
                        {
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            return Task.FromResult(0);
                        }));

                var toRecipient = new Recipient()
                {
                    EmailAddress = new EmailAddress()
                    {
                        Address = toRecipients[0]
                    }
                };

                // Create the message
                var actionableMessage = new Message()
                {
                    Subject = "There is a new visitor at the door",
                    ToRecipients = new List<Recipient>() { toRecipient },
                    Body = new ItemBody()
                    {
                        ContentType = BodyType.Html,
                        Content = LoadActionableMessageBody("VisitorAdaptiveCard.json", visitorImage, visitorName)
                    },
                    Attachments = new MessageAttachmentsCollectionPage()
                };

                // Create an attachment for the activity image
                var actionImage = new FileAttachment()
                {
                    ODataType = "#microsoft.graph.fileAttachment",
                    Name = "activity_image", // IMPORTANT: Name must match ContentId
                    IsInline = true,
                    ContentId = "activity_image",
                    ContentType = "image/jpg",
                    ContentBytes = System.IO.File.ReadAllBytes(@".\ActivityImage.jpg")
                };

                actionableMessage.Attachments.Add(actionImage);

                // Send the message
                await graphClient.Me.SendMail(actionableMessage, true).Request().PostAsync();
            }
            catch (Microsoft.Graph.ServiceException graphEx)
            {
                Console.WriteLine("An exception occurred while making a Graph request.");
                Console.WriteLine("  Code: {0}; Message: {1}", graphEx.Error.Code, graphEx.Message);
            }
        }

        static string LoadActionableMessageBody(string adaptiveCard, string visitorImage, string visitorName)
        {
            // Load the card JSON
            //string imageFile = string.Format("data:image/jpeg;base64,{0}", visitorImage);
            string cardText = System.IO.File.ReadAllText(@".\Helpers\" + adaptiveCard);
            //string cardString = string.Format(cardText, imageFile, visitorName);
            var cardJson = JObject.Parse(cardText);

            // Check type
            //var cardType = cardJson.SelectToken("type");

            string scriptType = "application/adaptivecard+json";

            // Insert empty originator guid
            string originatorId = "00000000-0000-0000-0000-000000000000";
            
            // First check if there is an existing originator value
            var originator = cardJson.SelectToken("originator");

            if (originator != null)
            {
                // Overwrite existing value
                cardJson["originator"] = originatorId;
            }
            else
            {
                // Add value
                cardJson.Add(new JProperty("originator", originatorId));
            }

            // Insert the JSON into the HTML
            return string.Format(System.IO.File.ReadAllText(@".\Helpers\MailBody.html"), scriptType, cardJson.ToString());
        }
    }
}
