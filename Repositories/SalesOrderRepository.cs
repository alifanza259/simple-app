using Microsoft.EntityFrameworkCore;
using SimpleApp.DTO;
using SimpleApp.Entities;

namespace SimpleApp.Repositories
{
    public class SalesOrderRepository : ISalesOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public SalesOrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> CreateSalesOrderAsync(OrderDto orderDto)
        {
            var soOrder = new SoOrder
            {
                OrderNo = orderDto.OrderNo,
                OrderDate = orderDto.OrderDate,
                ComCustomerId = orderDto.ComCustomerId,
                Address = orderDto.Address
            };

            var customer = await _context.ComCustomers.FindAsync(orderDto.ComCustomerId);

            if (customer == null)
            {
                throw new Exception("Customer Not Found");
            }

            soOrder.Customer = customer;

            await _context.SoOrders.AddAsync(soOrder);

            await _context.SaveChangesAsync();

            if (orderDto.Items != null && orderDto.Items.Any())
            {
                foreach (var itemDto in orderDto.Items)
                {
                    var item = new SoItem
                    {
                        ItemName = itemDto.ItemName,
                        Price = (int)itemDto.Price,
                        Quantity = itemDto.Quantity,
                        SoOrderId = (int)soOrder.SoOrderId
                    };
                    await _context.SoItems.AddAsync(item);
                }
                await _context.SaveChangesAsync();
            }


            return orderDto;
        }

        public async Task DeleteSalesOrderAsync(int id)
        {
            var order = await _context.SoOrders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.SoOrderId == id);

            if (order != null)
            {
                _context.SoItems.RemoveRange(order.Items);
                _context.SoOrders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<OrderDto> EditSalesOrderAsync(OrderDto orderDto)
        {
            var soOrder = await _context.SoOrders
                        .Include(o => o.Items)
                        .FirstOrDefaultAsync(o => o.SoOrderId == orderDto.SoOrderId);

            if (soOrder == null)
            {
                throw new Exception("Order not found.");
            }

            soOrder.OrderNo = orderDto.OrderNo;
            soOrder.OrderDate = orderDto.OrderDate;
            soOrder.ComCustomerId = orderDto.ComCustomerId;
            soOrder.Address = orderDto.Address;

            if (orderDto.Items != null)
            {

                foreach (var existingItem in soOrder.Items.ToList())
                {
                    if (!orderDto.Items.Any(i => i.SoItemId == existingItem.SoItemId))
                    {
                        _context.SoItems.Remove(existingItem);
                    }
                }

                foreach (var itemDto in orderDto.Items)
                {
                    if (itemDto.SoItemId > 0)
                    {
                        var existingItem = soOrder.Items.FirstOrDefault(i => i.SoItemId == itemDto.SoItemId);
                        if (existingItem != null)
                        {
                            existingItem.ItemName = itemDto.ItemName;
                            existingItem.Quantity = itemDto.Quantity;
                            existingItem.Price = (int)itemDto.Price;
                        }
                    }
                    else
                    {
                        var newItem = new SoItem
                        {
                            ItemName = itemDto.ItemName,
                            Quantity = itemDto.Quantity,
                            Price = (int)itemDto.Price
                        };
                        newItem.SoOrderId = soOrder.SoOrderId;
                        await _context.SoItems.AddAsync(newItem);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return orderDto;
        }

        public async Task<SoOrder> FindSalesOrderAsync(int id)
        {
            var soOrder = await _context.SoOrders.Include(so => so.Customer)
                .Include(so => so.Items)
                .FirstOrDefaultAsync(so => so.SoOrderId == id);

            return soOrder;
        }

        public IQueryable<SoOrder> GetSalesOrders(string searchKeyword, DateTime? orderDate)
        {
            var query = _context.SoOrders
                .Include(o => o.Customer)
                .OrderByDescending(o => o.SoOrderId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchKeyword))
            {
                query = query.Where(o => o.OrderNo.Contains(searchKeyword) || o.Customer.CustomerName.Contains(searchKeyword));
            }

            if (orderDate != null)
            {
                query = query.Where(o => o.OrderDate.Date == orderDate.Value.Date);
            }

            var orders = query.Select(e => new SoOrder
            {
                SoOrderId = e.SoOrderId,
                Address = e.Address,
                ComCustomerId = e.ComCustomerId,
                OrderDate = e.OrderDate,
                OrderNo = e.OrderNo,
                Customer = e.Customer
            });


            return orders;
        }

        public async Task<List<ComCustomer>> GetCustomerListAsync()
        {
            return await _context.ComCustomers.ToListAsync();
        }
    }
}
