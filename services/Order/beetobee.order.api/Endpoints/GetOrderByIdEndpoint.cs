using beetobee.order.application.Features.Orders.Queries;
using beetobee.order.api.Auth;

namespace beetobee.order.api.Endpoints;

public record GetOrderByIdResponse(OrderDto Order);
public class GetOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetOrderByIdQuery(id));
            var response = result.Adapt<GetOrderByIdResponse>();
            return Results.Ok(response);
        })
        .WithName("GetOrderById")
        .Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .WithSummary("Get order by ID")
        .WithDescription("Get a single order by its unique identifier")
        .RequireAuthorization(Permissions.ViewOrder);
    }
}
