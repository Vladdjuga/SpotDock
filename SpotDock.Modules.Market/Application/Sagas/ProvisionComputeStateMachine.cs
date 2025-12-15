using MassTransit;
using SpotDock.Modules.Market.Application.Contracts;

namespace SpotDock.Modules.Market.Application.Sagas;

public sealed class ProvisionComputeStateMachine : MassTransitStateMachine<ProvisionComputeSagaState>
{
    public State AwaitingFunds { get; private set; } = null!;
    public State AwaitingCapacity { get; private set; } = null!;
    public State AwaitingJobStart { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;

    public Event<RequestResources> RequestResourcesEvent { get; private set; } = null!;
    public Event<FundsAuthorized> FundsAuthorizedEvent { get; private set; } = null!;
    public Event<FundsRejected> FundsRejectedEvent { get; private set; } = null!;
    public Event<CapacityReserved> CapacityReservedEvent { get; private set; } = null!;
    public Event<CapacityUnavailable> CapacityUnavailableEvent { get; private set; } = null!;
    public Event<JobStarted> JobStartedEvent { get; private set; } = null!;
    public Event<JobStartFailed> JobStartFailedEvent { get; private set; } = null!;

    public ProvisionComputeStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => RequestResourcesEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Event(() => FundsAuthorizedEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Event(() => FundsRejectedEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Event(() => CapacityReservedEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Event(() => CapacityUnavailableEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Event(() => JobStartedEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Event(() => JobStartFailedEvent, x =>
        {
            x.CorrelateById(context => context.Message.RequestId);
        });

        Initially(
            When(RequestResourcesEvent)
                .Then(context =>
                {
                    context.Instance.RequestId = context.Message.RequestId;
                    context.Instance.UserId = context.Message.UserId;
                    context.Instance.CpuCores = context.Message.CpuCores;
                    context.Instance.MemoryMb = context.Message.MemoryMb;
                    context.Instance.CreatedAt = DateTime.UtcNow;
                })
                // TODO: send command to Billing: AuthorizeFunds
                .TransitionTo(AwaitingFunds)
        );

        During(AwaitingFunds,
            When(FundsAuthorizedEvent)
                // TODO: send command to Compute: ReserveCapacity
                .TransitionTo(AwaitingCapacity),

            When(FundsRejectedEvent)
                .TransitionTo(Failed)
        );

        During(AwaitingCapacity,
            When(CapacityReservedEvent)
                // TODO: send command to Compute: StartJob
                .TransitionTo(AwaitingJobStart),

            When(CapacityUnavailableEvent)
                // TODO: send compensation to Billing: ReleaseFunds
                .TransitionTo(Failed)
        );

        During(AwaitingJobStart,
            When(JobStartedEvent)
                .TransitionTo(Completed),

            When(JobStartFailedEvent)
                // TODO: send compensation: ReleaseFunds + ReleaseCapacity
                .TransitionTo(Failed)
        );

        SetCompletedWhenFinalized();
    }
}
