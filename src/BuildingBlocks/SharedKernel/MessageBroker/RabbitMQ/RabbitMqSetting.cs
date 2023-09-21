namespace SharedKernel.MessageBroker;

public class RabbitMqSetting
{
    public const string SectionName = "RabbitMQ";
    
    public string Host { get; set; }
    public ushort Port { get; set; }
    public string VirtualHost { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string QueuePrefix { get; set; }
}