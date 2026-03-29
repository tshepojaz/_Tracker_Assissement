using System.Linq;

namespace beetobee.order.application.Extentions;

public static class OrderExtention
{
    public static OrderDto ToOrderDto(this Order order)
    {
        return DtoOrder(order);
    }

    public static OrderDto DtoOrder(Order order)
    {
        return new OrderDto(
            OrderId: order.Id.Value,
            CustomerReference: order.CustomerReference,
            Items: order.OrderItems
                .Select(item => new OrderItemDto(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList()
        );
    }

}
