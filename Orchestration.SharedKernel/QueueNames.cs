namespace Orchestration.SharedKernel
{
    public static class QueueNames
    {
        public const string SagaOrchestrationQueue = "sagaorchestration";
        public const string CreditDemandFailedQueue = "creditdemandfailedqueue";
        public const string CreditDemandCompletedQueue = "creditdemandcompletedqueue";
        public const string CreditDemandHandledQueue = "creditdemandhandledqueue";
        public const string EvaluationFailedQueue = "evaluationfailedqueue";
        public const string MoneyTransferRequestedQueue = "moneytransferrequestedqueue";
        public const string EvaluationRollbackQueue = "evaluationrollbackqueue";

    }
}
