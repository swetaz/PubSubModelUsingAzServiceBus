using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using AzureServiceBusDemo.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AzureServiceBusDemo.Controllers
{
    public class PubSubController : Controller
    {
        string connectionString = "YOUR_CONNECTION_STRING_FROM_AZURE_SERVICE_BUS";
        string topicName = "YOUR_TOPIC_NAME_FROM_AZURE_SERVICE_BUS";
        ServiceBusClient client;
        public PubSubController()
        {
            if(connectionString!= "YOUR_CONNECTION_STRING_FROM_AZURE_SERVICE_BUS")
            {
                client = new ServiceBusClient(connectionString);
            }
        }
        public IActionResult Publisher()
        {
            SubscribeModel();
            return View();
        }
        public IActionResult Subscriber()
        {
            return View();
        }
        public async Task<JsonResult> PublishMsg(string msg)
        {
            ServiceBusSender senderObj = client.CreateSender(topicName);
            var msgbody = new TopicModel()
            {
                Message = msg,
                MsgTime = DateTime.UtcNow
            };

            string body = JsonConvert.SerializeObject(msgbody);
            var message = new ServiceBusMessage(body);
            await senderObj.SendMessageAsync(message);
            return Json(new { status = "success" });
        }
        private async void SubscribeModel()
        {
            if (connectionString != "YOUR_CONNECTION_STRING_FROM_AZURE_SERVICE_BUS")
            {
                await CreateSubsription("MobileApps");
                await CreateSubsription("IOTDevice");
                await CreateSubsription("Web");
            }

        }
        private async Task CreateSubsription(string subscriptionName)
        {
            var adminClient = new ServiceBusAdministrationClient(connectionString);

            if (!await adminClient.SubscriptionExistsAsync(topicName, subscriptionName).ConfigureAwait(false))
            {
                CreateSubscriptionOptions sp = new CreateSubscriptionOptions(topicName, subscriptionName);
                await adminClient.CreateSubscriptionAsync(sp);
            }
        }
        public async Task<List<TopicModel>> CheckSubscribedMsg(string env)
        {
            List<TopicModel> topicModels = new List<TopicModel>();
            var subscriptionName = env;

            var adminClient = new ServiceBusAdministrationClient(connectionString);
            ServiceBusReceiver receiverObj = client.CreateReceiver(topicName, subscriptionName);
            List<ServiceBusReceivedMessage> msgs = new List<ServiceBusReceivedMessage>();

            var receivedMessages = await receiverObj.ReceiveMessagesAsync(50, TimeSpan.FromSeconds(2));

            if (receivedMessages != null)
            {
                foreach (var receivedMessage in receivedMessages)
                {
                    string messageBody = Encoding.UTF8.GetString(receivedMessage.Body);
                    var body = JsonConvert.DeserializeObject<TopicModel>(messageBody);
                    topicModels.Add(body);
                    await receiverObj.CompleteMessageAsync(receivedMessage);
                }
            }
            return topicModels;
        }
    }
}
