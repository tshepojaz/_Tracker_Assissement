namespace beetobee.order.application.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(Guid orderId) 
        : base($"Order with ID {orderId} was not found.")
    {
    }
}
