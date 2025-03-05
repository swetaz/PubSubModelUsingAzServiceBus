# Implementing Publisher Subscriber Model Using Azure Service Bus
#PubSubModelUsingAzServiceBus

This example is a simple explantion on Publisher Subscriber model using Azure service bus - 

Publisher - Entity which publishes or send a message to a topic or channel.
Subcriber - Entity which subscribes or receives messages from a topic or channel.

To run the project please follow the following instructions - 
1. Please create a Azure Service bus resource.
2. Under that resource create a Topic and 3 subcriptions ("Web","IOT Device" and "Mobile Apps").
3. Please replace the topic name and connection string in PubSubController to run the project.

I have direclty mentioned the topic name and connection string in controller itself but it will be a good if you read from appSetting.

Thanks,
V Sweta
