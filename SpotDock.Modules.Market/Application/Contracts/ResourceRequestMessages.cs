namespace SpotDock.Modules.Market.Application.Contracts;

public interface RequestResources
{
    Guid RequestId { get; }
    Guid UserId { get; }
    int CpuCores { get; }
    int MemoryMb { get; }
}

public interface FundsAuthorized
{
    Guid RequestId { get; }
    Guid UserId { get; }
}

public interface FundsRejected
{
    Guid RequestId { get; }
    Guid UserId { get; }
    string Reason { get; }
}

public interface CapacityReserved
{
    Guid RequestId { get; }
}

public interface CapacityUnavailable
{
    Guid RequestId { get; }
    string Reason { get; }
}

public interface JobStarted
{
    Guid RequestId { get; }
    Guid AllocationId { get; }
}

public interface JobStartFailed
{
    Guid RequestId { get; }
    string Reason { get; }
}
