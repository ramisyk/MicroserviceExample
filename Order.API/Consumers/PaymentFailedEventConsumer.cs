using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers;

public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
{
    readonly OrderAPIDbContext _orderAPIDbContext;

    public PaymentFailedEventConsumer(OrderAPIDbContext orderApıDbContext)
    {
        _orderAPIDbContext = orderApıDbContext;
    }

    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        Order.API.Models.Entities.Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
        order.OrderStatus = Models.Enums.OrderStatus.Failed;
        await _orderAPIDbContext.SaveChangesAsync();
    }
}