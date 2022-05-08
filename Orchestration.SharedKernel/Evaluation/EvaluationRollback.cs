using MassTransit;

namespace Orchestration.SharedKernel.Evaluation
{
    public class EvaluationRollback : CorrelatedBy<Guid>
    {
        public EvaluationRollback(Guid correlationId)
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
