namespace Orchestration.SharedKernel.Credit
{
    public class CreditDemandCompleted
    {
        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
