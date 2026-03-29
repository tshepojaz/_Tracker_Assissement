using System;

namespace beetobee.order.application.Features.Orders.Queries;

public record GetOrderByIdQuery(Guid OrderId) : IQuery<GetOrderByIdResult>;
public record GetOrderByIdResult(OrderDto Order);
