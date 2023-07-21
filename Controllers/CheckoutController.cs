using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BakeNChew.Models;
using BakeNChew.Data;
using BakeNChew.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;

namespace BakeNChew.Controllers;

public class CheckoutController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly string? connString;

    public CheckoutController(ILogger<HomeController> logger,
                        // ApplicationDbContext db,
                        UserManager<ApplicationUser> userManager,
                        IConfiguration configuration)
    {
        _logger = logger;
        // _db = db;
        _userManager = userManager;
        connString = configuration.GetConnectionString("ApplicationDbContextConnection");
    }


    public async Task<IActionResult> Index()
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }
        try
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connString);
            using (var _db = new ApplicationDbContext(optionsBuilder.Options))
            {
                string customerEmail = _userManager.GetUserName(User).ToString();
                //get user cart Id -DETAILS
                var cartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == customerEmail);
                //get all cart items 
                IEnumerable<CartItem> itemsInCart = _db.CartItems.Where(e => e.CartId == cartInfo.CartId).ToList();

                IEnumerable<CartItem> finalI = itemsInCart;

                //get user data
                var user = await _userManager.GetUserAsync(HttpContext.User);
                // ProductModelView finalCart = new ProductModelView();
                // ProductModelView finalCart = new ProductModelView();
                ProductModelView finalCartList = new ProductModelView();
                {
                    finalCartList.Cartitems = finalI;
                    finalCartList.TotalPrice = cartInfo.CartPrice;
                    finalCartList.UserAddress = user.CustomerAddress;
                    finalCartList.UserPhone = user.PhoneNumber;
                    finalCartList.TotalQty = cartInfo.TotalProducts;
                    finalCartList.CartId = cartInfo.CartId;
                }
                //data for cartIcon
                ViewBag.TotalItemsInCart = cartInfo.TotalProducts;
                return View(finalCartList);
            }
        }
        catch (Exception e)
        {
            _logger.LogInformation($"{e}");

            return View();
        }
    }


    [HttpPost]
    public async Task<IActionResult> Index(ProductModelView model)
    {

        try
        {
            int tempOrderId = new Random().Next(1000, 9999);
            string Email = _userManager.GetUserName(User);


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connString);
            using (var _db = new ApplicationDbContext(optionsBuilder.Options))
            {
                OrderModel order = new OrderModel()
                {
                    CartId = model.CartId,
                    CustomerEmail = Email,
                    Totalquantity = model.TotalQty,
                    UserPhoneNumber = model.UserPhone,
                    UserAddress = model.UserAddress,
                    Totalprice = model.TotalPrice,
                    OrderId = tempOrderId,
                    DateCreated = DateTime.Now.ToUniversalTime(),
                };

                _db.Add(order);

                //update product qty
                var userCartItems = _db.CartItems.Where(e => e.CartId == model.CartId);

                foreach (var item in userCartItems) //O(n)
                {
                    using (var db = new ApplicationDbContext(optionsBuilder.Options))
                    {
                        var products = db.Products.SingleOrDefault(x => x.ProductId == item.ProductId);
                        // some.ForEach(a => a.status = true);
                        products.Qty = products.Qty - item.Quantity;
                        if (products.Qty < 1)
                        {
                            products.Availiblity = false;
                        }
                        _db.Update(products);
                    }
                }


                //empty cart
                _db.CartItems.RemoveRange(_db.CartItems.Where(e => e.CartId == model.CartId));
                _db.SaveChanges();

                //update user cart main
                var cartUpdate = _db.Carts.SingleOrDefault(e => e.CustomerEmail == Email);
                cartUpdate.CartPrice = 0;
                cartUpdate.TotalProducts = 0;

                _db.Update(cartUpdate);


                _db.SaveChanges();
                TempData["message"] = $"Order placed successfully, order Id is {tempOrderId}";

                TempData["Saved"] = $"Order placed successfully, order Id is {tempOrderId}";
                ViewBag.OrderMsg = $"Order placed successfully, order Id is {tempOrderId}";
                return RedirectToAction("Index", "Order");
            }
        }
        catch (Exception e)
        {
            TempData["message"] = $"Order was not placed"; //alert
                                                           // TempData["Saved"] = "Not placed";
            TempData["Saved"] = "Order has not been placed";

            return RedirectToAction("Index");
        }
    }

    // public IActionResult GetOrders()
    // {
    //     if (!User.Identity.IsAuthenticated)
    //     {
    //         return RedirectToAction("Login", "Account");
    //     }

    //     //get user 
    //     string Email = _userManager.GetUserName(User);

    //     var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
    //     optionsBuilder.UseNpgsql(connString);
    //     using (var _db = new ApplicationDbContext(optionsBuilder.Options))
    //     {
    //         var getOrders = _db.Orders.Where(e => e.CustomerEmail == Email);

    //         var getCartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == Email);

    //         ProductModelView orders = new ProductModelView()
    //         {
    //             orders = getOrders
    //         };

    //         //for navbar cartIcon data
    //         ViewBag.TotalItemsInCart = getCartInfo.TotalProducts;

    //         return View(orders);
    //     }
    // }


}