namespace beetobee.order.domain.Models;

public class Order : Entity<OrderId>
{
    public Guid CustomerReference { get; private set; } = default!;
    public IReadOnlyCollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

    public static Order Create(OrderId id,
                        Guid customerReference,
                        IReadOnlyCollection<OrderItem> orderItems)
    {
        var order = new Order
        {
            Id = id,
            CustomerReference = customerReference,
            OrderItems = orderItems
        };
        return order;
    }
}
