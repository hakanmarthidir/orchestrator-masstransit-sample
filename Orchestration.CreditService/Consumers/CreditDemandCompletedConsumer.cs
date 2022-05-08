using MassTransit;
using Orchestration.SharedKernel.Credit;

namespace Orchestration.CreditService.Consumers
{
    public class CreditDemandCompletedConsumer : IConsumer<CreditDemandCompleted>
    {
        public CreditDemandCompletedConsumer()
        {
            //dbcontext
        }
        public async Task Consume(ConsumeContext<CreditDemandCompleted> context)
        {
            //this is end of sequence
            if (context.Message != null)
            {
                var message = context.Message;


                //database operations : update due to provide the logic

                await Console.Out.WriteLineAsync($"Credit Operation Successfully Completed for : {message.DemandId}");
            }

        }
    }

}
