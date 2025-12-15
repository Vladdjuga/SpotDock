using MediatR;

namespace SpotDock.Modules.Market.Application.Handlers.Commands.SpotInstance;

public class ReserveSpotInstanceCommand:IRequestHandler<ReserveSpotInstanceCommand.Request,ReserveSpotInstanceCommand.Result>
{
    public record Request(Guid UserId,uint CpuCores,uint RamMb):IRequest<Result>;
    public record Result();
    
    
    
    public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
    {
        // TODO
        return new Result();
    }
}