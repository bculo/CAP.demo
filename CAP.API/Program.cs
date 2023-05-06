using CAP.Application.Consumers;
using Savorboard.CAP.InMemoryMessageQueue;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddCap(x =>
{
    /* IN MEMORY CONFIG
    x.UseInMemoryStorage();
    x.UseInMemoryMessageQueue();
    */
    
    x.UseRabbitMQ(opt=>
    {
        opt.HostName = "localhost";
        opt.Port = 5672;
        opt.Password = "pass";
        opt.UserName = "user";
        opt.ExchangeName = "TEST";
    });

    x.UseInMemoryStorage();
    
    x.UseDashboard();
});

builder.Services.AddTransient<OrderReceivedConsumer>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
