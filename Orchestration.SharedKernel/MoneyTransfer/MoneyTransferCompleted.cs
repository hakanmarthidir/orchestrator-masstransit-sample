using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.SharedKernel.MoneyTransfer
{
    public class MoneyTransferCompleted : CorrelatedBy<Guid>
    {
        public MoneyTransferCompleted(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get; }

        public Guid DemandId { get; set; }
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
    }
}
