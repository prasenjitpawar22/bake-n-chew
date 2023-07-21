using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BakeNChew.Models;

namespace BakeNChew.Models;
public class CartItem
{
    [Key]
    public string ItemId { get; set; }
    [ForeignKey("Carts")]
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now.ToUniversalTime();
    public int ProductId { get; set; }
    // public ProductModel? Product { get; set; }
    public string ProductImage { get; set; }
    public string ProductName { get; set; }
    public float ProductPrice { get; set; }
    public float TotalPrice { get; set; }
}