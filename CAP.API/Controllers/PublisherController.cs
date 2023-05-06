using CAP.Application.Constants;
using CAP.Application.Events;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace CAP.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PublisherController : ControllerBase
{
    private readonly ICapPublisher _publisher;
    
    public PublisherController(ICapPublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] MessageDto request)
    {
        await PersistOrder();
        
        await _publisher.PublishAsync(MessageQueue.ORDER_RECEIVED, new OrderReceived
        {
            Text = request.Text ?? "No-content"
        });
        
        return NoContent();
    }
    
    private async Task PersistOrder()
    {
        await Task.Delay(2000);
    }
}

public class MessageDto
{
    public string Text { get; set; }
}