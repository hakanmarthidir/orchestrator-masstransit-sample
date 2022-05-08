using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orchestration.SharedKernel.Evaluation
{
    public class EvaluationStarted : CorrelatedBy<Guid>
    {
        public EvaluationStarted(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
        public Guid CorrelationId { get;}

        public Guid DemandId { get; set; }

        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
