

using CAP.Application.Constants;
using CAP.Application.Events;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsumerController : ControllerBase
{
    private readonly ILogger<ConsumerController> _logger;

    public ConsumerController(ILogger<ConsumerController> logger)
    {
        _logger = logger;
    }
    
    [NonAction]
    [CapSubscribe(MessageQueue.ORDER_RECEIVED,  Group = "ControllerGroup")]
    public async Task ShowMessage(OrderReceived messageSent)
    {
        _logger.LogInformation("Order with text {0} received in {0}", messageSent.Text, nameof(ConsumerController));
    }
}