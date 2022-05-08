using MassTransit;
using Orchestration.SharedKernel.Credit;

namespace Orchestration.CreditService.Consumers
{
    public class CreditDemandFailedConsumer : IConsumer<CreditDemandFailed>
    {
        public CreditDemandFailedConsumer()
        {
            //dbcontext
        }
        public async Task Consume(ConsumeContext<CreditDemandFailed> context)
        {
            //this is end of sequence
            if (context.Message != null)
            {
                var message = context.Message;

                //database operations : update due to provide the logic

                await Console.Out.WriteLineAsync($"Credit Operation Failed for : {message.DemandId} reason is {message.Description}");
            }

        }
    }

}
