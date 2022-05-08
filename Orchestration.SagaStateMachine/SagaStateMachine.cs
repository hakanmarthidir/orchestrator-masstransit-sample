using MassTransit;
using Orchestration.SharedKernel;
using Orchestration.SharedKernel.Credit;
using Orchestration.SharedKernel.Evaluation;
using Orchestration.SharedKernel.MoneyTransfer;

namespace Orchestration.SagaStateMachine
{
    public class SagaStateMachine : MassTransit.MassTransitStateMachine<CreditState>
    {
        //1- Events
        public Event<CreditDemandCreated> CreditDemandCreated { get; set; }
        public Event<EvaluationCompleted> EvaluationCompleted { get; set; }
        public Event<EvaluationFailed> EvaluationFailed { get; set; }
        public Event<MoneyTransferCompleted> MoneyTransferCompleted { get; set; }
        public Event<MoneyTransferFailed> MoneyTransferFailed { get; set; }

        //2- States
        public State CreditDemandHandledDone { get; set; }
        public State EvaluationDone { get; set; }
        public State EvaluationNotDone { get; set; }
        public State MoneyTransferDone { get; set; }
        public State MoneyTransferNotDone { get; set; }

        //3- Event and State Combination Scenarios
        public SagaStateMachine()
        {
            InstanceState(instance => instance.State);

            //every credit demand will have CorrelarionId. 
            Event(() => CreditDemandCreated, demand => demand.CorrelateBy<Guid>(db => db.DemandId, _event => _event.Message.DemandId).SelectId(g => Guid.NewGuid()));
            Event(() => EvaluationCompleted, evaluation => evaluation.CorrelateById(_event => _event.Message.CorrelationId));
            Event(() => EvaluationFailed, evaluation => evaluation.CorrelateById(_event => _event.Message.CorrelationId));
            Event(() => MoneyTransferCompleted, evaluation => evaluation.CorrelateById(_event => _event.Message.CorrelationId));
            Event(() => MoneyTransferFailed, evaluation => evaluation.CorrelateById(_event => _event.Message.CorrelationId));

            //firstly creditservice sends this message from CreditController to start the sequence.
            //when is a condition 
            //then is an action
            Initially(When(CreditDemandCreated)
                .Then(context =>
                {
                    context.Instance.UserId = context.Data.UserId;
                    context.Instance.DemandId = context.Data.DemandId;
                    context.Instance.Amount = context.Data.Amount;
                    context.Instance.CreatedDate = DateTime.UtcNow;
                })
                .TransitionTo(CreditDemandHandledDone) //change state to CreditDemandHandledDone and start to evaluation
                .Send(new Uri($"queue:{QueueNames.CreditDemandHandledQueue}"), context =>
                new CreditDemandHandled(context.Instance.CorrelationId)
                {
                    DemandId = context.Data.DemandId,
                    Amount = context.Data.Amount,
                    UserId = context.Data.UserId
                }));

            //our state is CreditDemandHandledDone and
            //waiting for the Evaluation responses : expectations are EvaluationCompleted or EvaluationFailed 
            //if we got Completed then start to MoneyTransfer operation
            //if not send the failed message

            During(CreditDemandHandledDone,
                When(EvaluationCompleted).TransitionTo(EvaluationDone)
                .Send(new Uri($"queue:{QueueNames.MoneyTransferRequestedQueue}"),
                context => new MoneyTransferRequested(context.Instance.CorrelationId)
                {
                    DemandId = context.Data.DemandId,
                    Amount = context.Data.Amount,
                    UserId = context.Data.UserId
                }),
                When(EvaluationFailed).TransitionTo(EvaluationNotDone)
                .Send(new Uri($"queue:{QueueNames.CreditDemandFailedQueue}"), context => new CreditDemandFailed()
                {
                    DemandId = context.Data.DemandId,
                    Amount = context.Data.Amount,
                    UserId = context.Data.UserId,
                    Description = context.Data.Description
                }));


            //our state is EvaluationDone and
            //waiting the MoneyTransfer responses : expectations are MoneyTransferCompleted or MoneyTransferFailed 
            //if we got Completed it means all operations are completed -> Finalize() works
            //if not send the failed message

            During(EvaluationDone,
                When(MoneyTransferCompleted).TransitionTo(MoneyTransferDone)
                .Send(new Uri($"queue:{QueueNames.CreditDemandCompletedQueue}"),
                context => new CreditDemandCompleted
                {
                    DemandId = context.Data.DemandId,
                    Amount = context.Data.Amount,
                    UserId = context.Data.UserId
                })
                .Finalize(),

                When(MoneyTransferFailed).TransitionTo(MoneyTransferNotDone)
                .Send(new Uri($"queue:{QueueNames.CreditDemandFailedQueue}"), context => new CreditDemandFailed()
                {
                    DemandId = context.Data.DemandId,
                    Amount = context.Data.Amount,
                    UserId = context.Data.UserId,
                    Description = context.Data.Description
                })
                //if you need to rollback something from EvaluationService you can call another Send method like below.
                .Send(new Uri($"queue:{QueueNames.EvaluationRollbackQueue}"), context => new EvaluationRollback(context.Instance.CorrelationId)
                {
                    DemandId = context.Data.DemandId,
                    Amount = context.Data.Amount,
                    UserId = context.Data.UserId,
                    Description = context.Data.Description
                })

                );

            SetCompletedWhenFinalized();


        }

    }
}
