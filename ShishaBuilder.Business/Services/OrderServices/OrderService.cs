using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using ShishaBuilder.Core.DB;
using ShishaBuilder.Core.DTOs.OrderDtos;
using ShishaBuilder.Core.DTOs.TobaccoDtos;
using ShishaBuilder.Core.Enums;
using ShishaBuilder.Core.Models;
using ShishaBuilder.Core.Repositories.OrderRepositories;
using ShishaBuilder.Core.Services.MasterServices;
using ShishaBuilder.Core.Services.OrderServices;
using ShishaBuilder.Core.Services.TableServices;
using ShishaBuilder.Core.Services.TobaccoServices;

namespace ShishaBuilder.Business.Services.OrderServices;

public class OrderService : IOrderService
{
    private IOrderRepository orderRepository;
    private ITableService tableService;
    private IHookahService hookahService;
    private ITobaccoService tobaccoService;
    private IMasterService masterService;
    private AppDbContext context;

    public OrderService(
        IOrderRepository orderRepository,
        ITableService tableService,
        ITobaccoService tobaccoService,
        IHookahService hookahService,
        IMasterService masterService,
        AppDbContext context

    )
    {
        this.orderRepository = orderRepository;
        this.tableService = tableService;
        this.hookahService = hookahService;
        this.tobaccoService = tobaccoService;
        this.masterService = masterService;
        this.context = context;
    }
    public async Task<Order> AddOrderAsync(Order order)
    {
        if (order == null)
        {
            throw new ArgumentNullException(nameof(order), "Order cannot be null.");
        }
        return await orderRepository.AddOrderAsync(order);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await orderRepository.GetAllOrdersAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int id)
    {
        return await orderRepository.GetOrderByIdAsync(id);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        await orderRepository.UpdateOrderAsync(order);
    }
    public async Task<int> GetTotalOrdersCountAsync()
    {
        return await orderRepository.GetTotalOrdersCountAsync();
    }

    public async Task<Dictionary<int, int>> GetTobaccoUsageStatsAsync()
    {
        return await orderRepository.GetTobaccoUsageStatsAsync();
    }

    public async Task<AllOrdersViewModelDto> GetOrdersViewModelAsync(int orderId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new Exception("Order not found");

        var table = await tableService.GetByIdTableAsync(order.TableId);
        var hookah = await hookahService.GetByIdHookahAsync(order.HookahId);

        // Если нужно — можно загрузить мастера (если назначен)
        Master? master = order.MasterId != 0
            ? await masterService.GetMasterByIdAsync(order.MasterId)
            : null;

        var tobaccos = new List<TobaccoShowInfoViewModelDto>();
        foreach (var ot in order.OrderTobaccos)
        {
            var tobacco = await tobaccoService.GetTobaccoByIdAsync(ot.TobaccoId);
            tobaccos.Add(new TobaccoShowInfoViewModelDto
            {
                Name = tobacco.Name,
                Brand = tobacco.Brand,
                Percentage = ot.Percentage
            });
        }

        return new AllOrdersViewModelDto
        {
            Id = order.Id,
            Table = table,
            Hookah = hookah,
            CreatedAt = order.CreatedAt,
            Master = master,
            Tobaccos = tobaccos
        };
    }

    public async Task AssignOrderToMasterAsync(int orderId, int masterId)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order == null || order.MasterId != 0)
            throw new Exception("Order not found or already taken.");

        order.MasterId = masterId;
        order.OrderStatus = OrderStatus.InProgress;

        await context.SaveChangesAsync();
    }

    public async Task<List<AllOrdersViewModelDto>> GetOrdersByMasterAsync(int masterId)
    {
        var orders = await orderRepository.GetOrdersByMasterIdAsync(masterId);

        var result = new List<AllOrdersViewModelDto>();

        foreach (var order in orders)
        {
            var table = await tableService.GetByIdTableAsync(order.TableId);
            var hookah = await hookahService.GetByIdHookahAsync(order.HookahId);

            var tobaccos = new List<TobaccoShowInfoViewModelDto>();
            foreach (var ot in order.OrderTobaccos)
            {
                var tobacco = await tobaccoService.GetTobaccoByIdAsync(ot.TobaccoId);
                tobaccos.Add(new TobaccoShowInfoViewModelDto
                {
                    Name = tobacco.Name,
                    Brand = tobacco.Brand,
                    Percentage = ot.Percentage
                });
            }

            result.Add(new AllOrdersViewModelDto
            {
                Id = order.Id,
                Table = table,
                Hookah = hookah,
                CreatedAt = order.CreatedAt,
                Tobaccos = tobaccos,
                Master = null
            });
        }

        return result;
    }
    public async Task<bool> UpdateOrderStatusAsync(int orderId, int masterId, OrderStatus newStatus)
    {
        var order = await orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            return false;

        if (order.MasterId != masterId)
            return false;

        order.OrderStatus = newStatus;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<List<AllOrdersViewModelDto>> GetAllOrdersByMasterAsync(int masterId)
    {
        var orders = await orderRepository.GetAllOrdersByMasterIdAsync(masterId);

        var result = new List<AllOrdersViewModelDto>();

        foreach (var order in orders)
        {
            var table = await tableService.GetByIdTableAsync(order.TableId);
            var hookah = await hookahService.GetByIdHookahAsync(order.HookahId);

            var tobaccos = new List<TobaccoShowInfoViewModelDto>();
            foreach (var ot in order.OrderTobaccos)
            {
                var tobacco = await tobaccoService.GetTobaccoByIdAsync(ot.TobaccoId);
                tobaccos.Add(new TobaccoShowInfoViewModelDto
                {
                    Name = tobacco.Name,
                    Brand = tobacco.Brand,
                    Percentage = ot.Percentage
                });
            }

            result.Add(new AllOrdersViewModelDto
            {
                Id = order.Id,
                Table = table,
                Hookah = hookah,
                CreatedAt = order.CreatedAt,
                Tobaccos = tobaccos,
                Master = null
            });
        }
        return result;
    }
}
