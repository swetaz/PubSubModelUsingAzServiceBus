namespace AzureServiceBusDemo.Models
{
    public class TopicModel
    {
        public string Owner { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime MsgTime { get; set; }
    }
}
