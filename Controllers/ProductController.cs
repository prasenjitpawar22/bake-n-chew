using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BakeNChew.Models;
using BakeNChew.ViewModels;
using BakeNChew.Data;
using Microsoft.AspNetCore.Identity;

namespace BakeNChew.Controllers;

public class ProductController : Controller
{
    private ApplicationDbContext _db;
    private ILogger<ProductController> _logger;
    private UserManager<ApplicationUser> _userManager;

    public ProductController(ApplicationDbContext db, ILogger<ProductController> ilogger,
                        UserManager<ApplicationUser> userManager)

    {
        _db = db;
        _logger = ilogger;
        _userManager = userManager;

    }


    [HttpGet]
    public async Task<IActionResult> Index(string searchProduct)
    {
        //if searchProduct is null
        if (string.IsNullOrEmpty(searchProduct))
        {
            //get productList from database
            IEnumerable<ProductModel> dbProducts = _db.Products.ToList();
            ProductModelView productList = new ProductModelView()
            {
                Products = dbProducts
            };

            // _logger.LogInformation($"{searchProduct}");
            // return View(productList);
            // add this block to get number of items in cart
            try
            {

                string customerEmail = _userManager.GetUserName(User).ToString();
                //get user cart Id -DETAILS
                var cartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == customerEmail);
                var ItemsInCart = _db.Carts.SingleOrDefault(e => e.CartId == cartInfo.CartId);


                ViewBag.TotalItemsInCart = ItemsInCart.TotalProducts;
                return View(productList);
            }
            catch (Exception e)
            {
                return View(productList);
            }
        }
        else
        {
            //get searched products filtered
            IEnumerable<ProductModel> dbProducts = _db.Products.Where(p => p.ProductName.Contains(searchProduct.ToLower())
                                                    || p.Category == searchProduct.ToLower());
            ProductModelView productList = new ProductModelView()
            {
                Products = dbProducts
            };

            try
            {

                string customerEmail = _userManager.GetUserName(User).ToString();
                //get user cart Id -DETAILS
                var cartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == customerEmail);
                var ItemsInCart = _db.Carts.SingleOrDefault(e => e.CartId == cartInfo.CartId);


                ViewBag.TotalItemsInCart = ItemsInCart.TotalProducts;
                return View(productList);
            }
            catch (Exception e)
            {
                return View(productList);
            }

            // return View(productList);
        }
    }


    //get single product details
    public IActionResult GetProduct(int Id)
    {
        // var product = _db.Products.FirstOrDefault(p => p.ProductId == Id);
        var db_product = _db.Products.Where(p => p.ProductId == Id);

        ProductModelView product = new ProductModelView()
        {
            Products = db_product
        };

        try
        {

            string customerEmail = _userManager.GetUserName(User).ToString();
            //get user cart Id -DETAILS
            var cartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == customerEmail);
            var ItemsInCart = _db.Carts.SingleOrDefault(e => e.CartId == cartInfo.CartId);


            ViewBag.TotalItemsInCart = ItemsInCart.TotalProducts;
            return View(product);

        }
        catch (Exception e)
        {
            return View(product);
        }
    }


}