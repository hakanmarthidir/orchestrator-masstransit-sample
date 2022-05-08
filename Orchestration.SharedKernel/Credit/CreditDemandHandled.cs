using MassTransit;

namespace Orchestration.SharedKernel.Credit
{
    public class CreditDemandHandled : CorrelatedBy<Guid>
    {
        public CreditDemandHandled(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }

        public Guid UserId { get; set; }
        public Guid DemandId { get; set; }
        public decimal Amount { get; set; }
    }
}
