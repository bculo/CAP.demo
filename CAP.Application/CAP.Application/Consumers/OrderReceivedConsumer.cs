using CAP.Application.Constants;
using CAP.Application.Events;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace CAP.Application.Consumers;

public class OrderReceivedConsumer : ICapSubscribe
{
    private readonly ILogger<OrderReceivedConsumer> _logger;
    
    public OrderReceivedConsumer(ILogger<OrderReceivedConsumer> logger)
    {
        _logger = logger;
    }

    [CapSubscribe(MessageQueue.ORDER_RECEIVED, Group = "ServiceGroup")]
    public async Task ReceiveMessage(OrderReceived messageSent)
    {
        _logger.LogInformation("Order with text {0} received in {0}", messageSent.Text, nameof(OrderReceivedConsumer));
    }
}