using System.Text;
using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Shared;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/discounts")]
    [Authorize]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DiscountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // Generate random discount code
        private async Task<string> GenerateDiscountCode()
        {
            // Define the characters that can be used in the code
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            // Create a random number generator
            Random random = new Random();
            // Create a string builder to store the code
            StringBuilder code = new StringBuilder();
            // Generate 10 random characters and append them to the code
            for (int i = 0; i < 6; i++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }
            // New code
            var newCode = code.ToString();
            // Check if not unique call again
            if ((await _unitOfWork.Discount.GetAsync(d => d.Code == newCode) != null))
            {
                newCode = await GenerateDiscountCode();
            }
            // Return the code as a string
            return newCode;

        }

        // <-------Discount Actions------->
        [HttpGet]
        public async Task<IActionResult> DiscountList()
        {
            try
            {
                var discounts = await _unitOfWork.Discount.GetListAsync(c => c.IsDeleted != true);
                var discountsResult = _mapper.Map<IEnumerable<DiscountDto>>(discounts);
                return Ok(discountsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetDiscount([FromRoute] Guid id)
        {
            try
            {
                var discount = await _unitOfWork.Discount.GetAsync(c =>
                (c.Id == id && c.IsDeleted != true), "Products.ProductImages");
                if (discount == null)
                {
                    return NotFound();
                }
                var discountResult = _mapper.Map<DiscountDto>(discount);

                var otherProducts = await _unitOfWork.Product.GetListAsync(p =>
                    !p.Discounts.Any(d => d.Id == discount.Id)
                    && p.IsDeleted != true, "ProductImages");
                discountResult.OtherProducts = _mapper.Map<List<ProductDto>>(otherProducts);
                return Ok(discountResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        // Get discount by code for users
        [HttpGet]
        [Route("codes")]
        public async Task<IActionResult> GetDiscountByCode([FromQuery] string code)
        {
            try
            {
                var discount = await _unitOfWork.Discount.GetAsync(c =>
                (c.Code == code && c.IsDeleted != true && c.IsActive), "Products");
                if (discount == null)
                {
                    return NotFound();
                }
                var discountResult = _mapper.Map<DiscountDto>(discount);
                return Ok(discountResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }



        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddDiscount([FromBody] DiscountDto? discountDto)
        {
            try
            {
                if (discountDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Discount is invalid.");
                }
                discountDto.Id = Guid.NewGuid();
                Discount discount = _mapper.Map<Discount>(discountDto);
                discount.CreatedAt = DateTime.Now;
                discount.Code = await GenerateDiscountCode();

                bool result = await _unitOfWork.Discount.AddAsync(discount);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return Ok(discountDto);
                }
                return BadRequest("Something wrong happen, please try later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }



        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountDto discountDto)
        {
            try
            {
                if (discountDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var discount = await _unitOfWork.Discount.GetAsync(c => c.Id == discountDto.Id
                                && c.IsDeleted != true, "Products");
                if (discount == null)
                {
                    return NotFound();
                }
                // We will use other products to set the new products
                var newDiscountProducts = _mapper.Map<List<Product>>(discountDto.OtherProducts);

                if (discount.Products != newDiscountProducts && newDiscountProducts != null)
                {
                    // First remove all products.
                    discount.Products.RemoveAll(p => true);
                    // Update it with newDiscountProducts.
                    // We use this way because dicount.Products cann't be set.
                    foreach (var newProduct in newDiscountProducts)
                    {
                        var product = await _unitOfWork.Product.GetAsync(x => x.Id == newProduct.Id && x.IsDeleted != true);
                        discount.Products.Add(product);
                    }
                }

                // Update fields
                discount.Name = discountDto.Name;
                discount.DiscountPercent = discountDto.DiscountPercent;
                discount.IsActive = discountDto.IsActive;
                discount.UpdatedAt = DateTime.Now;

                await _unitOfWork.SaveAsync();
                return Ok(discountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }



        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] Guid id)
        {
            try
            {
                var discount = await _unitOfWork.Discount.GetAsync(c => c.Id == id && c.IsDeleted != true);
                if (discount == null)
                {
                    return NotFound("The discount does not exist");
                }

                // Delete the discount
                discount.IsDeleted = true;
                discount.DeletedAt = DateTime.Now;
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteDiscountRange([FromBody] DiscountDto[] discountDtos)
        {
            try
            {
                foreach (var discountDto in discountDtos)
                {
                    var discount = await _unitOfWork.Discount.GetAsync(c => c.Id == discountDto.Id && c.IsDeleted != true);
                    if (discount == null)
                    {
                        return NotFound("The discount does not exist");
                    }
                    // Delete the discount
                    discount.IsDeleted = true;
                    discount.DeletedAt = DateTime.Now;
                }

                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }
    }
}
