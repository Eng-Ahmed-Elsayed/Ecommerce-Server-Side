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
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        [Route("{id:Guid}")]
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

                var otherProducts = await _unitOfWork.Product
                    .GetListAsync(p =>
                    (p.DiscoutId != discount.Id && p.IsDeleted != true), "ProductImages");
                discountResult.OtherProducts = _mapper.Map<List<ProductDto>>(otherProducts);
                return Ok(discountResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
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
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }



        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateDiscount([FromBody] DiscountDto discountDto)
        {
            try
            {
                if (discountDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var discount = await _unitOfWork.Discount.GetAsync(c => c.Id == discountDto.Id);
                if (discount == null)
                {
                    return NotFound();
                }
                // We will use other products to set the new products
                var newDiscountProducts = _mapper.Map<List<Product>>(discountDto.OtherProducts);

                if (discount.Products != newDiscountProducts && newDiscountProducts != null)
                {
                    foreach (var productDto in newDiscountProducts)
                    {
                        var product = await _unitOfWork.Product.GetAsync(p => p.Id == productDto.Id);
                        product.DiscoutId = discountDto.Id;

                        // We will save one time after the discount
                        //await _unitOfWork.SaveAsync();
                    }
                }

                // Update fields
                discount.Name = discountDto.Name;
                discount.DiscountPercent = discountDto.DiscountPercent;
                discount.IsActive = discountDto.IsActive;
                //discount.Products = discountDto.Products;
                discount.UpdatedAt = DateTime.Now;

                await _unitOfWork.SaveAsync();
                return Ok(discountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }



        [HttpDelete]
        [Route("{id:Guid}")]
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
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
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
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
