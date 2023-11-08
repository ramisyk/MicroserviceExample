using MassTransit;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Events;
using Shared.Messages;
using Stock.API.Models;

namespace Stock.API.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    readonly ISendEndpointProvider _sendEndpointProvider;
    readonly IPublishEndpoint _publishEndpoint;
    private readonly StockAPIDbContext _dbContext;

    public OrderCreatedEventConsumer(ISendEndpointProvider sendEndpointProvider, IPublishEndpoint publishEndpoint,
        StockAPIDbContext dbContext)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _publishEndpoint = publishEndpoint;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        List<bool> stockResult = new();

        foreach (OrderItemMessage orderItem in context.Message.OrderItems)
        {
            stockResult.Add((_dbContext.Stocks.ToList()
                .Where(s => s.ProductId == orderItem.ProductId && s.Count >= orderItem.Count)).Any());
        }

        if (stockResult.TrueForAll(sr => sr.Equals(true)))
        {
            foreach (OrderItemMessage orderItem in context.Message.OrderItems)
            {
                Stock.API.Models.Entities.Stock stock =
                    await _dbContext.Stocks.FirstOrDefaultAsync(s => s.ProductId == orderItem.ProductId);

                stock.Count -= orderItem.Count;
                _dbContext.Stocks.Update(stock);
                await _dbContext.SaveChangesAsync();
                //await _stockCollection.FindOneAndReplaceAsync(s => s.ProductId == orderItem.ProductId, stock);
            }

            StockReservedEvent stockReservedEvent = new()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                TotalPrice = context.Message.TotalPrice,
            };
            
            // event sadece bir kuyruğa özel yapılıyorsa send işleminde sadece belirtilen kuyruğa iletilecektir.
            ISendEndpoint sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.Payment_StockReservedEventQueue}"));
        }
        else
        {
            StockNotReservedEvent stockNotReservedEvent = new()
            {
                BuyerId = context.Message.BuyerId,
                OrderId = context.Message.OrderId,
                Message = "..."
            };

            await _publishEndpoint.Publish(stockNotReservedEvent);
            await Console.Out.WriteLineAsync("Stok işlemleri başarısız...");
        }
        
    }
}