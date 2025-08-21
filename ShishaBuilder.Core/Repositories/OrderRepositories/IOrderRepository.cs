using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Repositories.OrderRepositories;

public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task <Order> AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);
    Task<int> GetTotalOrdersCountAsync();
    Task<Dictionary<int, int>> GetTobaccoUsageStatsAsync();

    Task<List<Order>> GetOrdersByMasterIdAsync(int masterId);
    Task<List<Order>> GetAllOrdersByMasterIdAsync(int masterId);

}
