using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto? categoryDto)
        {
            try
            {
                if (categoryDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Category is invaild.");
                }
                categoryDto.Id = Guid.NewGuid();
                Category category = _mapper.Map<Category>(categoryDto);
                bool result = await _unitOfWork.Category.AddAsync(category);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return Ok(categoryDto);
                }
                return BadRequest("Something wrong happen, please try later.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
