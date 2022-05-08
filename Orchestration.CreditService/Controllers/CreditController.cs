using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Orchestration.CreditService.Contracts;
using Orchestration.SharedKernel;
using Orchestration.SharedKernel.Credit;

namespace Orchestration.CreditService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditController : ControllerBase
    {

        private readonly ISendEndpointProvider _sendEndpointProvider;

        public CreditController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("CreditService...");
        }

        private Guid SaveCreditDemand(CreditDto model)
        {
            //firstly i need to persist my CreditDto as waiting status within database. 
            //...your codes here....
            //if data persistence is successfull then send your event to the publishers

            return Guid.NewGuid();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreditDto model)
        {
            
            var savedCreditId = this.SaveCreditDemand(model);

            CreditDemandCreated demandCreated = new CreditDemandCreated()
            {
                DemandId = savedCreditId,
                UserId = model.UserId,
                Amount = model.CreditAmount
            };

            //we are sending our message directly to the saga queue. saga will manage our other operations. 
            ISendEndpoint sendEndpoint = await this._sendEndpointProvider.GetSendEndpoint(new($"queue:{QueueNames.SagaOrchestrationQueue}"));
            await sendEndpoint.Send<CreditDemandCreated>(demandCreated).ConfigureAwait(false);
            
            return Ok("Succesfully requested...");
        }
    }
}
