using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Shared;
using Models.Models;


namespace ecommerce_server_side.Controllers
{
    [Route("api/user/payment")]
    [Authorize]
    [ApiController]
    public class UserPaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserPaymentController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // <-------UserPayment Actions------->

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetUserPayment([FromRoute] Guid id)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userPayment = await _unitOfWork.UserPayment.GetAsync(e =>
                (e.Id == id && e.UserId == userId && e.IsDeleted != true), "User");
                if (userPayment == null)
                {
                    return NotFound();
                }
                var userPaymentResult = _mapper.Map<UserPaymentDto>(userPayment);
                return Ok(userPaymentResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> UserPaymentList()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userPayments = await _unitOfWork.UserPayment.GetListAsync(e =>
                e.IsDeleted != true && e.UserId == userId);
                var userPaymentsResult = _mapper.Map<IEnumerable<UserPaymentDto>>(userPayments);
                return Ok(userPaymentsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserPayment([FromBody] UserPaymentDto? userPaymentDto)
        {
            try
            {
                if (userPaymentDto == null || !ModelState.IsValid)
                {
                    return BadRequest("UserPayment is invalid.");
                }

                userPaymentDto.Id = Guid.NewGuid();
                userPaymentDto.UserId = User.FindFirst("id")?.Value;
                UserPayment userPayment = _mapper.Map<UserPayment>(userPaymentDto);
                userPayment.CreatedAt = DateTime.Now;
                bool result = await _unitOfWork.UserPayment.AddAsync(userPayment);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return Ok(userPaymentDto);
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
        public async Task<IActionResult> UpdateUserPayment([FromBody] UserPaymentDto userPaymentDto)
        {
            try
            {
                if (userPaymentDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var userId = User.FindFirst("id")?.Value;
                var userPayment = await _unitOfWork.UserPayment.GetAsync(e =>
                e.Id == userPaymentDto.Id && e.UserId == userId);
                if (userPayment == null)
                {
                    return NotFound("User payment does not exist");
                }

                // Update fields
                userPayment.Name = userPaymentDto.Name;
                userPayment.Provider = userPaymentDto.Provider;
                userPayment.AccountNo = userPaymentDto.AccountNo;
                userPayment.Expiry = userPaymentDto.Expiry;
                userPayment.Cvv = userPaymentDto.Cvv;
                userPayment.UpdatedAt = DateTime.Now;

                await _unitOfWork.SaveAsync();
                return Ok(userPaymentDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteUserPayment([FromRoute] Guid id)
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                var userPayment = await _unitOfWork.UserPayment.GetAsync(e =>
                e.Id == id && e.IsDeleted != true && e.UserId == userId);
                if (userPayment == null)
                {
                    return NotFound("User payment does not exist");
                }

                // Delete the userPayment
                userPayment.IsDeleted = true;
                userPayment.DeletedAt = DateTime.Now;
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
