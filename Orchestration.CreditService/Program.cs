using MassTransit;
using Orchestration.CreditService.Consumers;
using Orchestration.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreditDemandFailedConsumer>();
    x.AddConsumer<CreditDemandCompletedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host("192.168.178.35", "orchestration", h =>
        {
            h.Username("tadev");
            h.Password("tadev");
        });

        cfg.ReceiveEndpoint(QueueNames.CreditDemandFailedQueue, c =>
        {
            c.ConfigureConsumer<CreditDemandFailedConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.CreditDemandCompletedQueue, c =>
        {
            c.ConfigureConsumer<CreditDemandCompletedConsumer>(context);
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
