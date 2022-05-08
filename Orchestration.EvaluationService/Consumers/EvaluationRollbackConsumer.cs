using MassTransit;
using Orchestration.SharedKernel.Evaluation;

namespace Orchestration.EvaluationService.Consumers
{
    public class EvaluationRollbackConsumer : IConsumer<EvaluationRollback>
    {
        public EvaluationRollbackConsumer()
        {
            
        }

        public async Task Consume(ConsumeContext<EvaluationRollback> context)
        {
            var message = context.Message;
            if (message != null)
            {
                await Console.Out.WriteLineAsync($"Evaluation was rollbacked : {message.CorrelationId} {message.DemandId}");

                // -----
                //Your Rollback Operations comes here regarding Evaluation.. 
                //data persistence 
                // -----


               
            }
        }
    }
}
