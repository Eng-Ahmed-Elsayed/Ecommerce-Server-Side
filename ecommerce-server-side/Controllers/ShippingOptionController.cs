using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Shared;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/shipping-option")]
    [Authorize]
    [ApiController]
    public class ShippingOptionController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ShippingOptionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // <-------ShippingOption Actions------->
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetShippingOption([FromRoute] Guid id)
        {
            try
            {
                var shippingOption = await _unitOfWork.ShippingOption.GetAsync(c =>
                (c.Id == id && c.IsDeleted != true));
                if (shippingOption == null)
                {
                    return NotFound();
                }
                var shippingOptionResult = _mapper.Map<ShippingOptionDto>(shippingOption);
                return Ok(shippingOptionResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShippingOptionList()
        {
            try
            {
                var shippingOptions = await _unitOfWork.ShippingOption.GetListAsync(c => c.IsDeleted != true);
                var shippingOptionsResult = _mapper.Map<IEnumerable<ShippingOptionDto>>(shippingOptions);
                return Ok(shippingOptionsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddShippingOption([FromBody] ShippingOptionDto? shippingOptionDto)
        {
            try
            {
                if (shippingOptionDto == null || !ModelState.IsValid)
                {
                    return BadRequest("ShippingOption is invalid.");
                }
                shippingOptionDto.Id = Guid.NewGuid();
                ShippingOption shippingOption = _mapper.Map<ShippingOption>(shippingOptionDto);
                shippingOption.CreatedAt = DateTime.Now;
                bool result = await _unitOfWork.ShippingOption.AddAsync(shippingOption);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return Ok(shippingOptionDto);
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
        public async Task<IActionResult> UpdateShippingOption([FromBody] ShippingOptionDto shippingOptionDto)
        {
            try
            {
                if (shippingOptionDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var shippingOption = await _unitOfWork.ShippingOption.GetAsync(c => c.Id == shippingOptionDto.Id);
                if (shippingOption == null)
                {
                    return NotFound();
                }

                // Update fields
                shippingOption.Cost = shippingOptionDto.Cost;
                shippingOption.Method = shippingOptionDto.Method;
                shippingOption.DeliveryTime = shippingOptionDto.DeliveryTime;
                shippingOption.UpdatedAt = DateTime.Now;

                await _unitOfWork.SaveAsync();
                return Ok(shippingOptionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteShippingOption([FromRoute] Guid id)
        {
            try
            {
                var shippingOption = await _unitOfWork.ShippingOption.GetAsync(c => c.Id == id && c.IsDeleted != true);
                if (shippingOption == null)
                {
                    return NotFound("The shipping option does not exist");
                }

                // Delete the shippingOption
                shippingOption.IsDeleted = true;
                shippingOption.DeletedAt = DateTime.Now;
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteShippingOptionRange([FromBody] ShippingOptionDto[] shippingOptionDtos)
        {
            try
            {
                foreach (var shippingOptionDto in shippingOptionDtos)
                {
                    var shippingOption = await _unitOfWork.ShippingOption.GetAsync(c => c.Id == shippingOptionDto.Id && c.IsDeleted != true);
                    if (shippingOption == null)
                    {
                        return NotFound("The shipping option does not exist");
                    }
                    // Delete the shippingOption
                    shippingOption.IsDeleted = true;
                    shippingOption.DeletedAt = DateTime.Now;
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
