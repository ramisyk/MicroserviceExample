﻿using MassTransit;
using Microsoft.EntityFrameworkCore;
using Order.API.Models;
using Shared.Events;

namespace Order.API.Consumers;

public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
{
    readonly OrderAPIDbContext _orderAPIDbContext;

    public PaymentCompletedEventConsumer(OrderAPIDbContext orderApıDbContext)
    {
        _orderAPIDbContext = orderApıDbContext;
    }

    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        Order.API.Models.Entities.Order order = await _orderAPIDbContext.Orders.FirstOrDefaultAsync(o => o.OrderId == context.Message.OrderId);
        order.OrderStatus = Models.Enums.OrderStatus.Completed;
        await _orderAPIDbContext.SaveChangesAsync();    }
}