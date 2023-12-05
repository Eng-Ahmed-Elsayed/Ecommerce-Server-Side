using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Customer;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/users/carts")]
    [Authorize]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShoppingCartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> CartReview()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var shoppingCart = await _unitOfWork.ShoppingCart.GetAsync(x =>
                    x.UserId == userId && x.IsDeleted != true
                    , "CartItems.Product.ProductImages,CartItems.Product.Inventory");

                    // If the user does not have shopping cart create one.
                    if (shoppingCart == null)
                    {
                        shoppingCart = await CreateShoppingCart(userId);
                        if (shoppingCart == null)
                        {
                            return BadRequest("Can't create a shopping cart.");
                        }
                    }
                    await DeleteNotAvailableCartItems(shoppingCart);
                    var shoppingCartResult = _mapper.Map<ShoppingCartDto>(shoppingCart);
                    return Ok(shoppingCartResult);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
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

        private async Task<ShoppingCart> CreateShoppingCart(string userId)
        {

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.Now,
            };
            var result = await _unitOfWork.ShoppingCart.AddAsync(shoppingCart);
            if (result)
            {
                await _unitOfWork.SaveAsync();
                return shoppingCart;
            }
            return null;

        }

        [HttpPost]
        public async Task<IActionResult> AddCartItem([FromBody] CartItemDto cartItemDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var shoppingCart = await _unitOfWork.ShoppingCart.GetAsync(x =>
                    x.UserId == userId && x.IsDeleted != true);
                    // If the user does not have shopping cart create one.
                    if (shoppingCart == null)
                    {
                        shoppingCart = await CreateShoppingCart(userId);
                        if (shoppingCart == null)
                        {
                            return BadRequest("Can't create a shopping cart.");
                        }
                    }
                    // Check if this product is not out of stock
                    Product product = await _unitOfWork.Product.GetAsync(x =>
                    x.Id == cartItemDto.ProductId && x.IsDeleted != true, "Inventory");
                    if (product == null || product.Inventory.Quantity <= 0)
                    {
                        return NotFound("Product is not available.");
                    }
                    // Add new cart item
                    cartItemDto.Id = Guid.NewGuid();
                    var cartItem = _mapper.Map<CartItem>(cartItemDto);
                    // If the cartItem quantity bigger than the inventory make it equals to the inventory
                    if (cartItemDto.Quantity > product.Inventory.Quantity)
                    {
                        cartItemDto.Quantity = product.Inventory.Quantity;
                    }
                    cartItem.ShoppingCartId = shoppingCart.Id;
                    cartItem.CreatedAt = DateTime.Now;
                    var result = await _unitOfWork.CartItem.AddAsync(cartItem);
                    if (result)
                    {
                        await _unitOfWork.SaveAsync();
                        return Ok(cartItemDto);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCartItem([FromBody] CartItemDto cartItemDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var shoppingCart = await _unitOfWork.ShoppingCart.GetAsync(x =>
                                        x.UserId == userId
                                        && x.IsDeleted != true,
                                        "CartItems.Product.Inventory");
                    if (shoppingCart != null)
                    {
                        var cartItem = await _unitOfWork.CartItem.GetAsync(x =>
                                    x.Id == cartItemDto.Id
                                    && x.ShoppingCartId == shoppingCart.Id
                                    && x.IsDeleted != true);
                        if (cartItem != null && cartItemDto.Quantity > 0)
                        {
                            // Can not update if the quantity is bigger than the inventory.
                            cartItem.Quantity = cartItemDto.Quantity < cartItem.Product.Inventory.Quantity
                            ? cartItemDto.Quantity
                            : cartItem.Product.Inventory.Quantity;
                            cartItem.UpdatedAt = DateTime.Now;
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

        //[Route("carts")]
        //[HttpPut]
        //public async Task<IActionResult> UpdateShoppingCart([FromBody] ShoppingCartDto shoppingCartDto)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var userId = User.FindFirst("id")?.Value;
        //            var shoppingCart = await _unitOfWork.ShoppingCart.GetAsync(x =>
        //                                x.Id == shoppingCartDto.Id
        //                                && x.UserId == userId
        //                                && x.IsDeleted != true);
        //            // Update card items.
        //            if (shoppingCart != null)
        //            {
        //                shoppingCartDto.CartItems.ForEach(async cartItemDto =>
        //                {
        //                    var cartItem = await _unitOfWork.CartItem.GetAsync(x =>
        //                            x.Id == cartItemDto.Id
        //                            && x.ShoppingCartId == shoppingCart.Id
        //                            && x.IsDeleted != true);
        //                    if (cartItem != null)
        //                    {
        //                        // Will not work after we UpdateAsync again but we do not need
        //                        // this end point for now and it would be updated later.
        //                        await _unitOfWork.CartItem.UpdateAsync(_mapper.Map<CartItem>(cartItemDto));
        //                    }
        //                });
        //                await _unitOfWork.SaveAsync();
        //                return Ok();
        //            }
        //        }
        //        return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
        //    }
        //}

        [Route("{id:Guid}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCartItem([FromRoute] Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var shoppingCart = await _unitOfWork.ShoppingCart.GetAsync(x =>
                                        x.UserId == userId
                                        && x.IsDeleted != true);

                    var cartItem = await _unitOfWork.CartItem.GetAsync(x =>
                                    x.Id == id
                                    && x.ShoppingCartId == shoppingCart.Id
                                    && x.IsDeleted != true);

                    if (cartItem != null)
                    {
                        cartItem.IsDeleted = true;
                        cartItem.DeletedAt = DateTime.Now;
                        await _unitOfWork.SaveAsync();
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }





    }
}
