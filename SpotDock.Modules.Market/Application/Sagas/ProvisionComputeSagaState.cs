using MassTransit;

namespace SpotDock.Modules.Market.Application.Sagas;

public sealed class ProvisionComputeSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public string CurrentState { get; set; } = string.Empty;

    public Guid RequestId { get; set; }
    public Guid UserId { get; set; }

    public int CpuCores { get; set; }
    public int MemoryMb { get; set; }

    public DateTime CreatedAt { get; set; }
}
