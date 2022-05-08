using MassTransit;
using Orchestration.EvaluationService.Consumers;
using Orchestration.SharedKernel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreditDemandHandledConsumer>();
    x.AddConsumer<EvaluationRollbackConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint(QueueNames.CreditDemandHandledQueue, c =>
        {
            c.ConfigureConsumer<CreditDemandHandledConsumer>(context);
        });

        cfg.ReceiveEndpoint(QueueNames.EvaluationRollbackQueue, c =>
        {
            c.ConfigureConsumer<EvaluationRollbackConsumer>(context);
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
