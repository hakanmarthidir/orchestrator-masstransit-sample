using MassTransit;
using Orchestration.SagaStateMachine;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configure =>
        {

            configure.AddSagaStateMachine<SagaStateMachine, CreditState>().InMemoryRepository();

            //or you can use other options such as EFCORE
            //configure.AddSagaStateMachine<SagaStateMachine, CreditState>().EntityFrameworkRepository(options =>
            //              {
            //                  options.AddDbContext<DbContext, MySagaContext>((provider, builder) =>
            //                  {
            //                      builder.UseSqlServer("YOUR CONNECTION STRING");
            //                  });
            //              });


            configure.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ReceiveEndpoint("sagaorchestration", e =>
                {
                    e.ConfigureSaga<CreditState>(provider);
                });

                cfg.Host("192.168.178.35", "orchestration", h =>
                {
                    h.Username("tadev");
                    h.Password("tadev");
                });
            }));
        });

        //services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
