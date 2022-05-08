using MassTransit;

namespace Orchestration.SharedKernel.MoneyTransfer
{
    public class MoneyTransferFailed : CorrelatedBy<Guid>
    {
        public MoneyTransferFailed(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }

        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}
