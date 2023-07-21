
using BakeNChew.Data;
using BakeNChew.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BakeNChew.Controllers;

public class OrderController : Controller
{

    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string? connString;

    public OrderController(ILogger<HomeController> logger,
                        ApplicationDbContext db,
                        UserManager<ApplicationUser> userManager,
                        IConfiguration configuration)
    {
        _logger = logger;
        _db = db;
        _userManager = userManager;
        // connString = configuration.GetConnectionString("ApplicationDbContextConnection");
    }


    public IActionResult Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        //get user 
        string Email = _userManager.GetUserName(User);

        var getOrders = _db.Orders.Where(e => e.CustomerEmail == Email);

        var getCartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == Email);

        ProductModelView orders = new ProductModelView()
        {
            orders = getOrders
        };

        //for navbar cartIcon data
        ViewBag.TotalItemsInCart = getCartInfo.TotalProducts;

        return View(orders);
    }

}