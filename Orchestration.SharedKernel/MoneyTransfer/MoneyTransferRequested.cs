using MassTransit;

namespace Orchestration.SharedKernel.MoneyTransfer
{
    public class MoneyTransferRequested : CorrelatedBy<Guid>
    {
        public MoneyTransferRequested(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }

        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
