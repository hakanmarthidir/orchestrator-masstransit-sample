namespace Orchestration.SharedKernel.Credit
{
    public class CreditDemandFailed
    {
        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}
