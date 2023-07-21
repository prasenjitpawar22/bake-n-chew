using System.ComponentModel.DataAnnotations;

using BakeNChew.Models;

namespace BakeNChew.ViewModels;
public class ProductModelView
{

    public IEnumerable<ProductModel> Products { get; set; }
    public string SearchProduct { get; set; } //SearchProduct string
    public int ProductId { get; set; } //single product id (add to cart)
    public int orderQty { get; set; }  // order qty (add to cart)


    //for cart
    // [Required]
    public IEnumerable<CartItem> Cartitems { get; set; }
    [Required]
    public float TotalPrice { get; set; }
    [Required]
    public int TotalQty { get; set; }
    [Required]
    public string UserAddress { get; set; }
    [Required]
    public String UserPhone { get; set; }
    [Required]
    public int CartId { get; set; }


    //for show orders
    public IEnumerable<OrderModel> orders { get; set; }
}