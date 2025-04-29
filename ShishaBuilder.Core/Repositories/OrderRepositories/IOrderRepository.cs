using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.Models;

namespace ShishaBuilder.Core.Repositories.OrderRepositories;

public interface IOrderRepository
{
    Task<Order> GetOrderByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task <Order> AddOrderAsync(Order order);
    Task UpdateOrderAsync(Order order);

}
