using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Shared;
using Models.Models;
using Utility.ManageFiles;

namespace ecommerce_server_side.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManageFiles _manageFiles;
        public CategoryController(IUnitOfWork unitOfWork,
            IMapper mapper,
            IManageFiles manageFiles)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _manageFiles = manageFiles;
        }

        // <-------Category Actions------->
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id)
        {
            try
            {
                var category = await _unitOfWork.Category.GetAsync(c =>
                (c.Id == id && c.IsDeleted != true));

                if (category == null)
                {
                    return NotFound();
                }

                var categoryResult = _mapper.Map<CategoryDto>(category);
                return Ok(categoryResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            try
            {
                var categories = await _unitOfWork.Category.GetListAsync(c => c.IsDeleted != true);
                var categoriesResult = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return Ok(categoriesResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto? categoryDto)
        {
            try
            {
                if (categoryDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Category is invalid.");
                }
                categoryDto.Id = Guid.NewGuid();
                Category category = _mapper.Map<Category>(categoryDto);
                category.CreatedAt = DateTime.Now;
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
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // Upload category image
        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Roles = "Administrator")]
        [Route("upload-file")]
        public async Task<IActionResult> UploadCategoryImage()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                if (file.Length > 0)
                {
                    var dbPath = _manageFiles.UploadFile(file, "Category");
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var category = await _unitOfWork.Category.GetAsync(c => c.Id == categoryDto.Id);
                if (category == null)
                {
                    return NotFound();
                }

                // Delete the old image
                if (category.ImgPath != categoryDto.ImgPath)
                {
                    _manageFiles.DeleteImage(category.ImgPath);
                    category.ImgPath = categoryDto.ImgPath;
                }

                // Update teh fields
                category.Name = categoryDto.Name;
                category.Description = categoryDto.Description;
                category.UpdatedAt = DateTime.Now;

                await _unitOfWork.SaveAsync();
                return Ok(categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }



        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            try
            {
                var category = await _unitOfWork.Category.GetAsync(c => c.Id == id && c.IsDeleted != true);
                if (category == null)
                {
                    return NotFound("The category is not exist");
                }
                // Delete the image first
                _manageFiles.DeleteImage(category.ImgPath);
                // Delete the category
                category.IsDeleted = true;
                category.DeletedAt = DateTime.Now;
                //await _unitOfWork.Category.DeleteAsync(category);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteCategoryRange([FromBody] CategoryDto[] categoryDtos)
        {
            try
            {
                foreach (var categoryDto in categoryDtos)
                {
                    var category = await _unitOfWork.Category.GetAsync(c => c.Id == categoryDto.Id && c.IsDeleted != true);
                    if (category == null)
                    {
                        return NotFound("The category is not exist");
                    }
                    // Delete the image first
                    _manageFiles.DeleteImage(category.ImgPath);
                    // Delete the category
                    category.IsDeleted = true;
                    category.DeletedAt = DateTime.Now;
                }

                //await _unitOfWork.Category.DeleteRangeAsync();
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
