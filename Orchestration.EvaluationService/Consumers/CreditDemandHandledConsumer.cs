using MassTransit;
using Orchestration.SharedKernel;
using Orchestration.SharedKernel.Credit;
using Orchestration.SharedKernel.Evaluation;

namespace Orchestration.EvaluationService.Consumers
{
    public class CreditDemandHandledConsumer : IConsumer<CreditDemandHandled>
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public CreditDemandHandledConsumer(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<CreditDemandHandled> context)
        {
            var message = context.Message;
            if (message != null)
            {
                await Console.Out.WriteLineAsync($"Evaluation is starting for : {message.CorrelationId} {message.DemandId} {message.UserId}");

                // -----
                //Your Operations comes here regarding Evaluation.. 
                //data persistence
                //some conditions and decide that it is ok or not. 
                // -----


                ISendEndpoint sendEndpoint = await this._sendEndpointProvider.GetSendEndpoint(new($"queue:{QueueNames.SagaOrchestrationQueue}"));

                //my dummy condition to test 
                if (message.Amount < 10000)
                {
                    // it is ok, go to the next step -> money transfer
                    //moneytransfer service should be subscribed this event

                    await Console.Out.WriteLineAsync($"Evaluation is completed for : {message.CorrelationId} {message.DemandId} {message.UserId}");

                    await sendEndpoint.Send<EvaluationCompleted>(
                        new EvaluationCompleted(message.CorrelationId)
                        {
                            Amount = message.Amount,
                            UserId = message.UserId,
                            DemandId = message.DemandId,
                            CompletedDate = DateTime.UtcNow
                        })
                        .ConfigureAwait(false);
                }
                else
                {
                    // it is failed raised failed event
                    //credit service should be subscribed this event
                    //you can choose Publish or Send scenario 
                    await Console.Out.WriteLineAsync($"Evaluation is failed for : {message.CorrelationId} {message.DemandId} {message.UserId}");

                    await sendEndpoint.Send<EvaluationFailed>(
                        new EvaluationFailed(message.CorrelationId)
                        {
                            Amount = message.Amount,
                            UserId = message.UserId,
                            DemandId = message.DemandId,
                            Description = "too much money was demanded, your score is negative."
                        })
                        .ConfigureAwait(false);
                }
            }
        }
    }
}
