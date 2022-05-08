namespace Orchestration.CreditService.Contracts
{
    public class CreditDto
    {
        public Guid UserId { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
