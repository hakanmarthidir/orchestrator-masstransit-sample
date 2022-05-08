using MassTransit;
using Orchestration.SharedKernel;
using Orchestration.SharedKernel.MoneyTransfer;

namespace Orchestration.MoneyTransferService.Consumers
{
    public class MoneyTransferRequestedConsumer : IConsumer<MoneyTransferRequested>
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MoneyTransferRequestedConsumer(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task Consume(ConsumeContext<MoneyTransferRequested> context)
        {
            var message = context.Message;
            if (message != null)
            {
                await Console.Out.WriteLineAsync($"MoneyTransfer is started for : {message.CorrelationId} {message.DemandId}");

                // -----
                //Your Operations comes here regarding MoneyTransfer.. 
                //data persistence
                //some conditions and decide that it is ok or not. 
                // -----

                ISendEndpoint sendEndpoint = await this._sendEndpointProvider.GetSendEndpoint(new($"queue:{QueueNames.SagaOrchestrationQueue}"));

                //my dummy condition to test 
                if (message.Amount % 2 == 0)
                {
                    await Console.Out.WriteLineAsync($"MoneyTransfer is completed for : {message.CorrelationId} {message.DemandId} {message.UserId}");
                    //transfer is ok 
                    await sendEndpoint.Send<MoneyTransferCompleted>(
                        new MoneyTransferCompleted(message.CorrelationId)
                        {
                            Amount = message.Amount,
                            UserId = message.UserId,
                            DemandId = message.DemandId,
                            TransferDate = DateTime.UtcNow
                        })
                        .ConfigureAwait(false);
                }
                else
                {
                    await Console.Out.WriteLineAsync($"MoneyTransfer is failed for : {message.CorrelationId} {message.DemandId} {message.UserId}");

                    await sendEndpoint.Send<MoneyTransferFailed>(
                        new MoneyTransferFailed(message.CorrelationId)
                        {
                            Amount = message.Amount,
                            UserId = message.UserId,
                            DemandId = message.DemandId,
                            Description = "transfer was failed by authorities... :)"
                        })
                        .ConfigureAwait(false);
                }
            }
        }
    }
}
