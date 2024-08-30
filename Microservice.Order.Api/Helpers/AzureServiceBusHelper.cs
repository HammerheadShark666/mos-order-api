﻿using Azure.Messaging.ServiceBus;
using Microservice.Order.Api.Helpers.Interfaces;

namespace Microservice.Order.Api.Helpers;

public class AzureServiceBusHelper : IAzureServiceBusHelper
{
    public async Task SendMessage(string queue, string data)
    {
        var client = new ServiceBusClient(EnvironmentVariables.AzureServiceBusConnection);
        var sender = client.CreateSender(queue);

        await sender.SendMessageAsync(new ServiceBusMessage(data));
    }
}