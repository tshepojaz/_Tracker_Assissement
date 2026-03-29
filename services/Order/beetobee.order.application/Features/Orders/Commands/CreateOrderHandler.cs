using beetobee.order.application.Services;
using System.Net;
using Refit;

namespace beetobee.order.application.Features.Orders.Commands;

public class CreateOrderHandler(IApplicationDbContext dbContext, IProductService productService, ILogger<CreateOrderHandler> logger) 
           : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating order for customer {CustomerId}", request.Order.CustomerReference);

        // Validate each requested product against Catalog availability before creating the order.
        var requestedProducts = request.Order.Items
            .GroupBy(item => item.ProductId)
            .Select(group => new
            {
                ProductId = group.Key,
                RequestedQuantity = group.Sum(item => item.Quantity)
            });

        foreach (var requested in requestedProducts)
        {
            ProductResponseDto productResponse;
            try
            {
                productResponse = await productService.GetProductByIdAsync(
                    requested.ProductId.ToString(),
                    cancellationToken);
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                logger.LogWarning("Product {ProductId} was not found in catalog", requested.ProductId);
                throw new NotFoundException($"Product with ID {requested.ProductId} was not found.");
            }

            if (productResponse.product.availableQuantity < requested.RequestedQuantity)
            {
                logger.LogWarning(
                    "Insufficient stock for product {ProductId}. Requested {RequestedQuantity}, available {AvailableQuantity}",
                    requested.ProductId,
                    requested.RequestedQuantity,
                    productResponse.product.availableQuantity);

                throw new BadRequestException(
                    $"Insufficient quantity for product {requested.ProductId}. Requested: {requested.RequestedQuantity}, Available: {productResponse.product.availableQuantity}");
            }
        }

        var newOrder = CreateNewOrder(request.Order);
        dbContext.Orders.Add(newOrder);
        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Successfully created order {OrderId} for customer {CustomerId}",
            newOrder.Id.Value,
            request.Order.CustomerReference);
        return new CreateOrderResult(newOrder.Id.Value);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerReference: orderDto.CustomerReference,
            orderItems: orderDto.Items
                .Select(item => new OrderItem(item.ProductId, item.Quantity, item.UnitPrice))
                .ToList()
        );
        return newOrder;
    }
}
