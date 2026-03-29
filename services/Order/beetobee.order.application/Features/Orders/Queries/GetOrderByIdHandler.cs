namespace beetobee.order.application.Features.Orders.Queries;

public class GetOrderByIdHandler(IApplicationDbContext dbContext, ILogger<GetOrderByIdHandler> logger)
    : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching order with ID {OrderId}", request.OrderId);

        var order = await dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == OrderId.Of(request.OrderId), cancellationToken);

        if (order == null)
        {
            logger.LogWarning("Order with ID {OrderId} not found", request.OrderId);
            throw new OrderNotFoundException(request.OrderId);
        }

        return new GetOrderByIdResult(order.ToOrderDto());
    }
}

