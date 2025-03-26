using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SimpleApp.DTO;
using SimpleApp.Entities;
using SimpleApp.Models;
using SimpleApp.Repositories;
using System.Text.Json;

namespace SimpleApp.Controllers
{
    public class SalesOrderController : Controller
    {
        private readonly ISalesOrderRepository _salesOrderRepository;
        public SalesOrderController(ISalesOrderRepository salesOrderRepository)
        {
            _salesOrderRepository = salesOrderRepository;
        }

        public async Task<IActionResult> Index(string searchKeyword, DateTime? orderDate, int pageNumber = 1)
        {
            var data = _salesOrderRepository.GetSalesOrders(searchKeyword, orderDate);
            
            ViewBag.Keyword = searchKeyword;
            ViewBag.OrderDate = orderDate?.ToString("yyyy-MM-dd");

            var pageSize = 5;
            return View(await PaginatedList<SoOrder>.CreateAsync(data, pageNumber, pageSize));
        }

        public async Task<IActionResult> Add()
        {
            var customers = await _salesOrderRepository.GetCustomerListAsync();

            var order = new OrderDto
            {
                Customers = customers.Select(c => new CustomerDto
                {
                    ComCustomerId = c.ComCustomerId,
                    CustomerName = c.CustomerName
                }).ToList(),
                OrderDate = DateTime.Now
            };

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Add(OrderDto orderDto, string Items)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Items))
                {
                    orderDto.Items = JsonSerializer.Deserialize<List<ItemDto>>(Items, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }

                var result = await _salesOrderRepository.CreateSalesOrderAsync(orderDto);
                return RedirectToAction(nameof(Index));
            }

            var customers = await _salesOrderRepository.GetCustomerListAsync();

            var order = new OrderDto
            {
                Customers = customers.Select(c => new CustomerDto
                {
                    ComCustomerId = c.ComCustomerId,
                    CustomerName = c.CustomerName
                }).ToList(),
                OrderDate = DateTime.Now
            };
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _salesOrderRepository.DeleteSalesOrderAsync(id);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id, int? pageNumber)
        {
            var soOrder = await _salesOrderRepository.FindSalesOrderAsync(id);
            if (soOrder == null)
            {
                return NotFound();
            }

            var customers = await _salesOrderRepository.GetCustomerListAsync();

            var order = new OrderDto
            {
                SoOrderId = soOrder.SoOrderId,
                OrderNo = soOrder.OrderNo,
                OrderDate = soOrder.OrderDate,
                ComCustomerId = soOrder.ComCustomerId,
                CustomerName = soOrder.Customer.CustomerName,
                Address = soOrder.Address
            };

            var items = soOrder.Items.Select(i => new ItemDto
            {
                ItemName = i.ItemName,
                Price = i.Price,
                Quantity = i.Quantity,
                SoItemId = i.SoItemId
            });

            order.Items = items.ToList();
            order.Customers = customers.Select(c => new CustomerDto
            {
                ComCustomerId = c.ComCustomerId,
                CustomerName = c.CustomerName
            }).ToList();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(OrderDto orderDto, string Items)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(Items))
                {
                    orderDto.Items = JsonSerializer.Deserialize<List<ItemDto>>(Items, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                var result = await _salesOrderRepository.EditSalesOrderAsync(orderDto);
                return RedirectToAction(nameof(Index));
            }

            var customers = await _salesOrderRepository.GetCustomerListAsync();
            var order = new OrderDto
            {
                Customers = customers.Select(c => new CustomerDto
                {
                    ComCustomerId = c.ComCustomerId,
                    CustomerName = c.CustomerName
                }).ToList(),
                OrderDate = DateTime.Now
            };

            return View(order);
        }

        public async Task<IActionResult> ExportIntoExcel(string searchKeyword, DateTime? orderDate)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Alif");

            using (var package = new ExcelPackage())
            {
                var data = await _salesOrderRepository.GetSalesOrders(searchKeyword, orderDate).ToListAsync();

                var worksheet = package.Workbook.Worksheets.Add("SalesOrders");

                worksheet.Cells["A1"].Value = "No";
                worksheet.Cells["B1"].Value = "Sales Order";
                worksheet.Cells["C1"].Value = "Order Date";
                worksheet.Cells["D1"].Value = "Customer";

                for (var i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = i + 1;
                    worksheet.Cells[i + 2, 2].Value = data[i].OrderNo;
                    worksheet.Cells[i + 2, 3].Value = data[i].OrderDate.ToString("dd-MM-yyyy");
                    worksheet.Cells[i + 2, 4].Value = data[i].Customer.CustomerName;
                }

                var stream = new MemoryStream(package.GetAsByteArray());

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "sales-orders.xlsx");
            }
        }
    }
}
