using System.Net.Http.Headers;
using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;

        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-file")]
        public async Task<IActionResult> UploadFile()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var file = formCollection.Files.First();
                var folderName = Path.Combine("StaticFiles", "Images", "Category");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file.Length > 0)
                {
                    var dbPath = SaveFile(file, pathToSave, folderName);
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

        [HttpPost, DisableRequestSizeLimit]
        [Route("upload-files")]
        public IActionResult UploadFiles()
        {
            try
            {
                Request.Form.Files.Count();
                var files = Request.Form.Files;
                var folderName = Path.Combine("StaticFiles", "Images", "Product");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }
                var dbPathList = new List<string>();
                foreach (var file in files)
                {
                    var dbPath = SaveFile(file, pathToSave, folderName);
                    dbPathList.Add(dbPath);
                }
                return Ok(new { dbPathList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Save file
        private string SaveFile(IFormFile? file, string pathToSave, string folderName)
        {
            var fileName = Guid.NewGuid().ToString() + ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return dbPath;
        }

        // Delete image with path
        private void DeleteImage(string imgPath)
        {

            if (!imgPath.IsNullOrEmpty())
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), imgPath);
                System.IO.File.Delete(fullPath);
            }

        }

        // <-------Category Actions------->
        [HttpPost]
        [Route("category/add")]
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



        [HttpPut]
        [Route("category/{id:guid}")]
        public async Task<IActionResult> UpdateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invaild Model!");
                }

                var category = await _unitOfWork.Category.GetAsync(c => c.Id == categoryDto.Id);
                if (category == null)
                {
                    return NotFound();
                }

                // Delete the old image
                if (category.ImgPath != categoryDto.ImgPath)
                {
                    DeleteImage(category.ImgPath);
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

        [HttpGet]
        [Route("category/list")]
        public async Task<IActionResult> CategoryList()
        {
            try
            {
                //var categories = await _unitOfWork.Category.GetListAsync();
                var categories = await _unitOfWork.Category.GetListAsync(c => c.IsDeleted != true);
                var categoriesResult = _mapper.Map<IEnumerable<CategoryDto>>(categories);
                return Ok(categoriesResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        [Route("category/{id:Guid}")]
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

        [HttpDelete]
        [Route("category/{id:Guid}")]
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
                DeleteImage(category.ImgPath);
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
        [Route("category")]
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
                    DeleteImage(category.ImgPath);
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





//[HttpDelete]
//[Route("{id:Guid}")]
//public async Task<IActionResult> DeleteEmployee([FromRoute] string id)
//{
//    Employee employee = await _unitOfWork.Employee.GetAsync(emp => emp.Id == id);
//    if (employee != null)
//    {
//        _unitOfWork.Employee.Remove(employee);
//        await _unitOfWork.SaveAsync();
//        EmployeeDto employeeResult = _mapper.Map<EmployeeDto>(employee);
//        return Ok(employee);
//    }
//    return NotFound(id);

//}

//[HttpGet("privacy")]
//[Authorize(Roles = "Administrator")]

//public IActionResult Privacy()
//{
//    var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
//    return Ok(claims);
//}

//    }