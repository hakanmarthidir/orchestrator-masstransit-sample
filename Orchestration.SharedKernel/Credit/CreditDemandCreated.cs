namespace Orchestration.SharedKernel.Credit
{
    public class CreditDemandCreated
    {
        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
