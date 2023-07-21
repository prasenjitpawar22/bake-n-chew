using System.ComponentModel.DataAnnotations;



namespace BakeNChew.Models;

public class ProductModel
{

    [Key]
    public int ProductId { get; set; }
    public String ProductName { get; set; }
    public String Shape { get; set; }
    public String Flavour { get; set; }
    public float Price { get; set; }
    public bool Availiblity { get; set; }
    public string Image { get; set; }
    public string Category { get; set; }
    public int Qty { get; set; }
}