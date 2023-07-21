using BakeNChew.Data;
using BakeNChew.Models;
using BakeNChew.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BakeNChew.Controllers;
public class CartController : Controller
{
    private ApplicationDbContext _db;
    private ILogger<ProductController> _logger;
    private UserManager<ApplicationUser> _userManager;

    public CartController(
        ApplicationDbContext db,
        ILogger<ProductController> ilogger,
        UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _logger = ilogger;
        _userManager = userManager;
    }


    // [Authorize() ]
    // [Route("/account/login")]
    //add to cart

    // [HttpPost]
    public IActionResult AddToCart(ProductModelView model, string returnUrl)
    {


        var Id = model.ProductId;
        var QtyOrder = model.orderQty;

        var popup = false;
        //if user not logged in 
        if (!User.Identity.IsAuthenticated)
        {
            //Redirect to Login...

            return RedirectToAction("Login", "Account");
        }

        var productPrice = _db.Products.SingleOrDefault(e => e.ProductId == Id);

        // _logger.LogInformation($" innnn {QtyOrder} ");

        //if ordered qty is greater then available 
        if (productPrice.Qty < QtyOrder)
        {
            //pop up error
            _logger.LogInformation($" innnn {QtyOrder} ");
            // popup 
            TempData["message"] = $"You can order maximum quantity upto {productPrice.Qty}";
            return RedirectToAction("Index", "Product");
            // return RedirectToAction(null); ;
        }

        //--------
        // if logged in then
        //--------

        // --------------------------------------------------
        //if the item is already in the cart
        var checkItem = _db.CartItems.SingleOrDefault(e => e.ProductId == Id);


        if (checkItem != null)
        {
            try
            {
                // get user email id
                _logger.LogInformation($"{checkItem}");
                checkItem.Quantity += QtyOrder;
                checkItem.TotalPrice = productPrice.Price * checkItem.Quantity;
                _db.Update(checkItem);

                // //update the product table data
                // productPrice.Qty -= QtyOrder;
                // if (productPrice.Qty == 0)
                // {
                //     productPrice.Availiblity = false;
                // }

                // _db.Update(productPrice);



                //update the cart items 
                var cartItemsDetails = _db.CartItems.Where(e => e.CartId == checkItem.CartId).
                                                        Select(t => t.TotalPrice);
                float cartSum = cartItemsDetails.Sum();
                var cartCount = cartItemsDetails.Count();


                var cart = _db.Carts.SingleOrDefault(e => e.CartId == checkItem.CartId);
                cart.TotalProducts = cartCount;
                cart.CartPrice = cartSum;

                _db.Update(cart);
                _db.SaveChanges();


                return RedirectToAction("Index", "Product");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Product");
            }
        }

        else
        {
            try
            {
                //-------------------------------------------------------------------------------
                // if the item is not in the cart add to cartItems

                //generate cartItem Id
                string tempCartItemsId = new Random().Next(1000, 9999).ToString();
                // Get user email id:
                string customerEmail = _userManager.GetUserName(User).ToString();
                //get user cart Id -DETAILS
                var cartInfo = _db.Carts.SingleOrDefault(e => e.CustomerEmail == customerEmail);

                //get product price 
                var productInfo = _db.Products.SingleOrDefault(e => e.ProductId == Id);
                CartItem cartItems = new CartItem
                {
                    ItemId = tempCartItemsId,
                    DateCreated = DateTime.Now.ToUniversalTime(),
                    CartId = cartInfo.CartId,
                    ProductId = Id,
                    ProductName = productInfo.ProductName,
                    ProductPrice = productInfo.Price,
                    Quantity = QtyOrder,
                    TotalPrice = productInfo.Price * QtyOrder,
                    ProductImage = productInfo.Image
                };

                _db.Add(cartItems);

                // //update the product table data
                // productInfo.Qty -= QtyOrder;
                // if (productInfo.Qty == 0)
                // {
                //     productInfo.Availiblity = false;
                // }

                _db.Update(productInfo);
                _db.SaveChanges();

                //get list of all items

                //update the cart items
                var cartItemsDetails = _db.CartItems.Where(e => e.CartId == cartInfo.CartId).
                                                        Select(t => t.TotalPrice);
                float cartSum = cartItemsDetails.Sum();
                var cartCount = cartItemsDetails.Count();

                var cart = _db.Carts.SingleOrDefault(e => e.CartId == cartInfo.CartId);
                cart.TotalProducts = cartCount;
                cart.CartPrice = cartSum;

                _db.Update(cart);
                _db.SaveChanges();

                return RedirectToAction("Index", "Product");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Product");
            }
        }



        //get all items which has the cartId of user
        // var cartItemsFinal = _db.CartItems.Where(e => e.CartId == cartInfo.CartId);

        // total sum of qty and price (two variable)
        // in cart table add total price and total sum 

        //Finally
        //just update the CART table Data

    }
}