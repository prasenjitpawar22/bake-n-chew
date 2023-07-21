using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BakeNChew.Data;
using BakeNChew.Models;

namespace BakeNChew.Models;

public class CartModel
{
    [Key]
    public int CartId { get; set; }

    // [ForeignKey("CartItems")]
    // public int CartItemsId { get; set; }

    // [ForeignKey("ProductId")]
    // public ProductModel Products { get; set; }
    // [ForeignKey("UserId")]
    // public string CustomerId { get; set; }

    public string CustomerEmail { get; set; }
    public float CartPrice { get; set; }
    public DateTime DateCreated { get; set; }
    public int TotalProducts { get; set; }

}