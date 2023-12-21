using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DataTransferObjects.Customer;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/users/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }


        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] OrderDetailsDto orderDetailsDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    // Every property should be exist and belong to the same uesr.
                    var shoppingCart = await _unitOfWork.ShoppingCart.GetAsync(x =>
                                        x.UserId == userId
                                        && x.IsDeleted != true,
                                        "CartItems.Product.Inventory,CartItems.Product.Discounts");
                    var userAddress = await _unitOfWork.UserAddress.GetAsync(e =>
                                        e.Id == orderDetailsDto.UserAddressId
                                        && e.UserId == userId
                                        && e.IsDeleted != true);
                    var userPayment = await _unitOfWork.UserPayment.GetAsync(e =>
                                    e.Id == orderDetailsDto.UserPaymentId
                                    && e.UserId == userId
                                    && e.IsDeleted != true);

                    // Check 1-if there is a shopping cart.
                    // 2- The shopping cart has cart items.
                    // 3- There is an available address for this user.
                    // 4- There is an available payment for this user.
                    if (shoppingCart != null
                        && shoppingCart.CartItems.Count() > 0
                        && userAddress != null
                        && userPayment != null)
                    {
                        // Delete all unavailable Cart Items.
                        await DeleteNotAvailableCartItems(shoppingCart);
                        // New Id for the order
                        var orderDetailsId = Guid.NewGuid();
                        // Create New Order Details With New Order Items(cartItem => orderItem).
                        shoppingCart.CartItems.ForEach(async cartItem =>
                        {
                            // Update the inventory quantity
                            // Check if the Inventory.Quantity > cartItem.Quantity
                            if (cartItem.Product.Inventory.Quantity > cartItem.Quantity)
                            {
                                cartItem.Product.Inventory.Quantity -= cartItem.Quantity;

                            }
                            else
                            {
                                cartItem.Quantity = cartItem.Product.Inventory.Quantity;
                                cartItem.Product.Inventory.Quantity = 0;
                            }
                            // New orderItem
                            OrderItem orderItem = _mapper.Map<OrderItem>(cartItem);
                            orderItem.OrderDetailsId = orderDetailsId;
                            orderItem.CreatedAt = DateTime.Now;
                            // If there is a discount code check 
                            if (!orderDetailsDto.DiscountCode.IsNullOrEmpty())
                            {
                                // If it is available for this item apply it.
                                var dis = orderItem.Product.Discounts.Find(d =>
                                    d.Code == orderDetailsDto.DiscountCode);
                                if (dis != null && dis.IsActive)
                                {
                                    orderItem.DiscountCode = dis.Code;
                                    orderItem.DiscountPercent = dis.DiscountPercent;
                                }

                            }
                            await _unitOfWork.OrderItem.AddAsync(orderItem);
                        });
                        OrderDetails orderDetails = _mapper.Map<OrderDetails>(orderDetailsDto);
                        orderDetails.Id = orderDetailsId;
                        orderDetails.CreatedAt = DateTime.Now;
                        orderDetails.UserId = userId;
                        var result = await _unitOfWork.OrderDetails.AddAsync(orderDetails);
                        if (result)
                        {
                            // Delete old shopping cart
                            shoppingCart.IsDeleted = true;
                            shoppingCart.DeletedAt = DateTime.Now;

                            await _unitOfWork.SaveAsync();
                            return Ok();
                        }
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var orders = await _unitOfWork.OrderDetails.GetListAsync(x =>
                                x.UserId == userId
                                && x.IsDeleted != true,
                                "OrderItems.Product.ProductImages");
                if (orders != null)
                {
                    var ordersResult = _mapper.Map<IEnumerable<OrderDetailsDto>>(orders);
                    return Ok(orders);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "The server encountered an unexpected condition. Please try again later.");
            }
        }

        // Check if the cart item and the product is available first
        // and if not delete it from the cart.
        private async Task DeleteNotAvailableCartItems(ShoppingCart shoppingCart)
        {
            shoppingCart.CartItems.RemoveAll(x =>
                                            x.IsDeleted == true
                                            || x.Product.IsDeleted == true
                                            || x.Product.Inventory.Quantity <= 0);
            await _unitOfWork.SaveAsync();
        }
    }
}
