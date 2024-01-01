using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Shared;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/users/addresses")]
    [Authorize]
    [ApiController]
    public class UserAddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserAddressController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // <-------UserAddress Actions------->

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserAddress([FromRoute] Guid id)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userAddress = await _unitOfWork.UserAddress.GetAsync(e =>
                (e.Id == id && e.UserId == userId && e.IsDeleted != true), "User");
                if (userAddress == null)
                {
                    return NotFound();
                }
                var userAddressResult = _mapper.Map<UserAddressDto>(userAddress);
                return Ok(userAddressResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserAddressList()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userAddresses = await _unitOfWork.UserAddress.GetListAsync(e =>
                e.IsDeleted != true && e.UserId == userId);
                var userAddressesResult = _mapper.Map<IEnumerable<UserAddressDto>>(userAddresses);
                return Ok(userAddressesResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAddress([FromBody] UserAddressDto? userAddressDto)
        {
            try
            {
                if (userAddressDto == null || !ModelState.IsValid)
                {
                    return BadRequest("UserAddress is invalid.");
                }

                userAddressDto.Id = Guid.NewGuid();
                userAddressDto.UserId = User.FindFirst("id")?.Value;
                UserAddress userAddress = _mapper.Map<UserAddress>(userAddressDto);
                userAddress.CreatedAt = DateTime.Now;
                bool result = await _unitOfWork.UserAddress.AddAsync(userAddress);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return Ok(userAddressDto);
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
        public async Task<IActionResult> UpdateUserAddress([FromBody] UserAddressDto userAddressDto)
        {
            try
            {
                if (userAddressDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var userId = User.FindFirst("id")?.Value;
                var userAddress = await _unitOfWork.UserAddress.GetAsync(e =>
                e.Id == userAddressDto.Id && e.UserId == userId && e.IsDeleted != true);
                if (userAddress == null)
                {
                    return NotFound("User address does not exist");
                }

                // Update fields
                userAddress.FirstName = userAddressDto.FirstName;
                userAddress.LastName = userAddressDto.LastName;
                userAddress.AddressLine1 = userAddressDto.AddressLine1;
                userAddress.City = userAddressDto.City;
                userAddress.State = userAddressDto.State;
                userAddress.Country = userAddressDto.Country;
                userAddress.PostalCode = userAddressDto.PostalCode;
                userAddress.Mobile = userAddressDto.Mobile;
                // Not required fields
                userAddress.AddressLine2 = userAddressDto.AddressLine2 != null
                    ? userAddressDto.AddressLine2 : userAddress.AddressLine2;
                userAddress.Telephone = userAddressDto.Telephone != null
                    ? userAddressDto.Telephone : userAddress.Telephone;

                userAddress.UpdatedAt = DateTime.Now;

                await _unitOfWork.SaveAsync();
                return Ok(userAddressDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteUserAddress([FromRoute] Guid id)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userAddress = await _unitOfWork.UserAddress.GetAsync(e =>
                e.Id == id && e.IsDeleted != true && e.UserId == userId);
                if (userAddress == null)
                {
                    return NotFound("User address does not exist");
                }

                // Delete the userAddress
                userAddress.IsDeleted = true;
                userAddress.DeletedAt = DateTime.Now;
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
