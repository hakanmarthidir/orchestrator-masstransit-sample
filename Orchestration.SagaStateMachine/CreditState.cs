using MassTransit;

namespace Orchestration.SagaStateMachine
{
    public class CreditState : SagaStateMachineInstance
    {
        //CorrelationId was implemented by SagaStateMachineInstance to uniqueness
        public Guid CorrelationId { get; set; }
        public string State { get; set; }
        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
