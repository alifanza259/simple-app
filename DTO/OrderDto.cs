using System.ComponentModel.DataAnnotations;

namespace SimpleApp.DTO;

public class OrderDto
{
    public long SoOrderId { get; set; }

    [Required]
    public string OrderNo { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }

    [Required]
    public int ComCustomerId { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string? Address { get; set; }

    public List<CustomerDto> Customers { get; set; } = new();
    public List<ItemDto> Items { get; set; } = new List<ItemDto>();
}
