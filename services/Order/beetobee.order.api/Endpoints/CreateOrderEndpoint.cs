using commonblock.Idempotencies;
using beetobee.order.api.Auth;

namespace beetobee.order.api.Endpoints;

record CreateOrderRequest(OrderDto Order);
record CreateOrderResponse(Guid Id);
public class CreateOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders", async (CreateOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateOrderResponse>();
            return Results.Created($"/api/orders/{result.Id}", response);
        })
        .WithName("CreateOrder")
        .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Create a new order")
        .WithDescription("Create a new order")
        .AddEndpointFilter<IdempotencyFilter>()
        .RequireAuthorization(Permissions.CreateOrder);
    }
}
