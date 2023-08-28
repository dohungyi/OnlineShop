namespace MessageBroker.RabbitMQ;

public static class MessageQueueSettingExtensions
{
    public static string GetPublishEndpoint(this MessageQueueSettings settings, string endpoint)
    {
        var virHost = settings.VirtualHost.Trim().TrimStart('/').TrimEnd('/');
        virHost = string.IsNullOrWhiteSpace(virHost) ? virHost : $"/{virHost}";
        return $"rabbitmq://{settings.Host}:{settings.Port}{virHost}/{endpoint}";
    }

    public static string GetPublishEndpointError(this MessageQueueSettings settings, string endpoint)
    {
        var virHost = settings.VirtualHost.Trim().TrimStart('/').TrimEnd('/');
        virHost = string.IsNullOrWhiteSpace(virHost) ? virHost : $"/{virHost}";
        return $"rabbitmq://{settings.Host}:{settings.Port}{virHost}/{endpoint}_error";
    }

    public static string GetReceiveEndpoint(this MessageQueueSettings settings, string endpoint)
    {
        return $"{settings.QueuePrefix}{endpoint}";
    }

    public static string GetReceiveEndpointError(this MessageQueueSettings settings, string endpoint)
    {
        return $"{settings.QueuePrefix}{endpoint}_error";
    }

    public static Uri GetEndpointAddress(this MessageQueueSettings settings, string endpoint)
    {
        return new Uri(settings.GetPublishEndpoint(endpoint));
    }
}