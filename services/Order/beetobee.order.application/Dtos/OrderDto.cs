namespace beetobee.order.application.Dtos;

public record OrderDto(
    Guid OrderId,
    Guid CustomerReference,
    List<OrderItemDto> Items
);
