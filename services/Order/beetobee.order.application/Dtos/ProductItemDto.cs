namespace beetobee.order.application.Dtos;

public record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice);
