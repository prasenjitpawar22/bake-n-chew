using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BakeNChew.Data;

namespace BakeNChew.Models;

public class OrderModel
{
    [Key]
    public int OrderId { get; set; }
    // [ForeignKey("AspNetUsers")]
    [Required]
    public string CustomerEmail { get; set; }
    [Required]
    public float Totalprice { get; set; }
    [Required]
    public int Totalquantity { get; set; }
    [Required]
    public string UserAddress { get; set; }
    [Required]
    public string UserPhoneNumber { get; set; }
    [Required]
    public int CartId { get; set; }

    public DateTime DateCreated { get; set; }

    // public string Deliverymethod { get; set; }
    // public string Paymentmethod { get; set; }
    // public ApplicationUser  { get; set; }
}