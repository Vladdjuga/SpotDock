using MediatR;

namespace SpotDock.Modules.Auctions.Application.Handlers.Commands.Bid;

public class PlaceBidCommand:IRequestHandler<PlaceBidCommand.Request,PlaceBidCommand.Result>
{
    public record Request(Guid UserId, decimal Amount):IRequest<Result>;
    public record Result();
    
    
    
    public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
    {
        // TODO
        return new Result();
    }
}