using MassTransit;
using Orchestration.MoneyTransferService.Consumers;
using Orchestration.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MoneyTransferRequestedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.ReceiveEndpoint(QueueNames.MoneyTransferRequestedQueue, c =>
        {
            c.ConfigureConsumer<MoneyTransferRequestedConsumer>(context);
        });

        cfg.Host("192.168.178.35", "orchestration", h =>
        {
            h.Username("tadev");
            h.Password("tadev");
        });

        cfg.ConfigureEndpoints(context);
    });

}


);


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
