using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Shared;
using Models.Models;


namespace ecommerce_server_side.Controllers
{
    [Route("api/user-payment")]
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
                var userPayment = await _unitOfWork.UserPayment.GetAsync(c =>
                (c.Id == id && c.IsDeleted != true), "User");
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
        [Route("list")]
        public async Task<IActionResult> UserPaymentList()
        {
            try
            {
                var userPayments = await _unitOfWork.UserPayment.GetListAsync(c => c.IsDeleted != true);
                var userPaymentsResult = _mapper.Map<IEnumerable<UserPaymentDto>>(userPayments);
                return Ok(userPaymentsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddUserPayment([FromBody] UserPaymentDto? userPaymentDto)
        {
            try
            {
                if (userPaymentDto == null || !ModelState.IsValid)
                {
                    return BadRequest("UserPayment is invaild.");
                }
                userPaymentDto.Id = Guid.NewGuid();
                UserPayment userPayment = _mapper.Map<UserPayment>(userPaymentDto);
                userPayment.CreatedAt = DateTime.Now;
                userPayment.Provider = "Test Provider";
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
                    return BadRequest("Invaild Model!");
                }

                var userPayment = await _unitOfWork.UserPayment.GetAsync(c => c.Id == userPaymentDto.Id);
                if (userPayment == null)
                {
                    return NotFound();
                }

                // Update fields
                userPayment.Name = userPaymentDto.Name;
                //userPayment.Provider = userPaymentDto.Provider;
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
                var userPayment = await _unitOfWork.UserPayment.GetAsync(c => c.Id == id && c.IsDeleted != true);
                if (userPayment == null)
                {
                    return NotFound("The userPayment is not exist");
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

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> DeleteUserPaymentRange([FromBody] UserPaymentDto[] userPaymentDtos)
        {
            try
            {
                foreach (var userPaymentDto in userPaymentDtos)
                {
                    var userPayment = await _unitOfWork.UserPayment.GetAsync(c => c.Id == userPaymentDto.Id && c.IsDeleted != true);
                    if (userPayment == null)
                    {
                        return NotFound("The userPayment does not exist");
                    }
                    // Delete the userPayment
                    userPayment.IsDeleted = true;
                    userPayment.DeletedAt = DateTime.Now;
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
