using MassTransit;

namespace Orchestration.SharedKernel.Evaluation
{
    public class EvaluationCompleted : CorrelatedBy<Guid>
    {
        public EvaluationCompleted(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }
        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CompletedDate { get; set; }
        public decimal Amount { get; set; }
    }
}
