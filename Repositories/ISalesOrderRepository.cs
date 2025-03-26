using SimpleApp.DTO;
using SimpleApp.Entities;

namespace SimpleApp.Repositories
{
    public interface ISalesOrderRepository
    {
        public IQueryable<SoOrder> GetSalesOrders(string searchKeyword, DateTime? orderDate);
        public Task<SoOrder> FindSalesOrderAsync(int id);
        public Task<OrderDto> CreateSalesOrderAsync(OrderDto orderDto);
        public Task<OrderDto> EditSalesOrderAsync(OrderDto orderDto);
        public Task DeleteSalesOrderAsync(int id);
        public Task<List<ComCustomer>> GetCustomerListAsync();
    }
}
