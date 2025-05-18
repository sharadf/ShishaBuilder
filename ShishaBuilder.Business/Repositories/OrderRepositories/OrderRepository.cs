using Microsoft.EntityFrameworkCore;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.OrderRepositories;

namespace ShishaBuilder.Business.Repositories.OrderRepositories;

public class OrderRepository : IOrderRepository
{
    private AppDbContext context;

    public OrderRepository(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            // Добавляем заказ вместе с табаками одним вызовом
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();

            await transaction.CommitAsync();
            return order;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception($"Error saving order: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await context
            .Orders.Include(o => o.OrderTobaccos) // Добавляем загрузку табаков
            .ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await context
            .Orders.Include(o => o.OrderTobaccos)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<int> GetTotalOrdersCountAsync()
    {
        return await context.Orders.CountAsync();
    }

    public async Task<Dictionary<int, int>> GetTobaccoUsageStatsAsync()
    {
        return await context.OrderTobaccos
            .GroupBy(x => x.TobaccoId)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);
    }


    public async Task UpdateOrderAsync(Order order)
    {
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var existingOrder = await context
                .Orders.Include(o => o.OrderTobaccos)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            if (existingOrder == null)
                throw new Exception("Order not found");

            // Обновляем скалярные свойства
            context.Entry(existingOrder).CurrentValues.SetValues(order);

            // Обрабатываем табаки
            await UpdateOrderTobaccosAsync(existingOrder, order.OrderTobaccos);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task UpdateOrderTobaccosAsync(
        Order existingOrder,
        ICollection<OrderTobacco> newTobaccos
    )
    {
        // Удаляем табаки, которых нет в новом списке
        var tobaccosToRemove = existingOrder
            .OrderTobaccos.Where(ot => !newTobaccos.Any(x => x.TobaccoId == ot.TobaccoId))
            .ToList();

        foreach (var tobacco in tobaccosToRemove)
        {
            context.OrderTobaccos.Remove(tobacco);
        }

        // Обновляем/добавляем табаки
        foreach (var newTobacco in newTobaccos)
        {
            var existingTobacco = existingOrder.OrderTobaccos.FirstOrDefault(ot =>
                ot.TobaccoId == newTobacco.TobaccoId
            );

            if (existingTobacco != null)
            {
                // Обновляем только необходимые поля
                existingTobacco.Percentage = newTobacco.Percentage;
            }
            else
            {
                // Добавляем новый табак
                existingOrder.OrderTobaccos.Add(
                    new OrderTobacco
                    {
                        TobaccoId = newTobacco.TobaccoId,
                        Percentage = newTobacco.Percentage,
                        OrderId = existingOrder.Id, // Важно явно указать OrderId
                    }
                );
            }
        }
    }
    public async Task<List<Order>> GetOrdersByMasterIdAsync(int masterId)
    {
        return await context.Orders
            .Include(o => o.OrderTobaccos)
            .Where(o => o.MasterId == masterId && o.OrderStatus == OrderStatus.InProgress)
            .ToListAsync();
    }

}
