using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BakeNChew.Models;
using BakeNChew.Data;
using BakeNChew.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BakeNChew.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger,
                        ApplicationDbContext db,
                        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        //get productList from database
        IEnumerable<ProductModel> dbProducts = _db.Products.ToList();
        ProductModelView productList = new ProductModelView()
        {
            Products = dbProducts
        };


        //NAVBAR cart Count: get the items count from cart table
        // pass  it viewbag 
        try
        {

            string customerEmail = _userManager.GetUserName(User).ToString();
            //get user cart Id -DETAILS
            var cartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == customerEmail);
            var ItemsInCart = _db.Carts.SingleOrDefault(e => e.CartId == cartInfo.CartId);


            //get all cart items 
            var listOfItems = _db.CartItems.Where(e => e.CartId == ItemsInCart.CartId);


            //data for cartIcon
            ViewBag.TotalItemsInCart = ItemsInCart.TotalProducts;
            return View(productList);
        }
        catch (Exception e)
        {
            return View(productList);
        }

    }

    //---about us page
    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // add to cart
    // public IActionResult AddToCart(int Id)
    // {
    //     _logger.LogInformation($"{Id}");
    //     return View();
    // }

}
