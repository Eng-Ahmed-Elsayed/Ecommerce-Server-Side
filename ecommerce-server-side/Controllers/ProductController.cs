using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DataTransferObjects;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }


        // <-------Product Actions------->
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            try
            {
                var product = await _unitOfWork.Product.GetAsync(p =>
                (p.Id == id && p.IsDeleted != true));
                if (product == null)
                {
                    return NotFound();
                }

                var productResult = _mapper.Map<ProductDto>(product);
                return Ok(productResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> ProductList()
        {
            try
            {
                //var categories = await _unitOfWork.Product.GetListAsync();
                var products = await _unitOfWork.Product.GetListAsync(p => p.IsDeleted != true, "ProductImages,Tags,Colors");
                var productsResult = _mapper.Map<IEnumerable<ProductDto>>(products);
                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto? productDto)
        {
            try
            {
                if (productDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Product is invaild.");
                }
                productDto.Id = Guid.NewGuid();
                Product product = _mapper.Map<Product>(productDto);
                // Add to inventory
                Inventory inventory = new Inventory()
                {
                    Id = Guid.NewGuid(),
                    Quantity = productDto.Quantity,
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.Inventory.AddAsync(inventory);
                product.Tags = await UpdateProductTagsAsync(productDto.Tags);
                product.Colors = await UpdateProductColorsAsync(productDto.Colors);
                product.InventoryId = inventory.Id;
                product.CreatedAt = DateTime.Now;
                bool result = await _unitOfWork.Product.AddAsync(product);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    return Ok();
                }
                return BadRequest("Something wrong happen, please try later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        // If the tag is new add it to the db then add to the tags list finally return this list.
        private async Task<List<Tag>> UpdateProductTagsAsync(List<Tag>? tagsDto)
        {
            List<Tag> tags = new List<Tag>();
            foreach (var tagDto in tagsDto)
            {
                var tag = await _unitOfWork.Tag.GetAsync(t => t.Name == tagDto.Name);
                if (tag == null)
                {
                    tagDto.Id = Guid.NewGuid();
                    await _unitOfWork.Tag.AddAsync(tagDto);
                    await _unitOfWork.SaveAsync();
                    tags.Add(tagDto);
                }
                else
                {
                    tags.Add(tag);
                }
            }
            return tags;
        }

        // If the color is new add it to the db then add to the colors list finally return this list.
        private async Task<List<Color>> UpdateProductColorsAsync(List<Color>? colorsDto)
        {
            List<Color> colors = new List<Color>();
            foreach (var colorDto in colorsDto)
            {
                var color = await _unitOfWork.Color.GetAsync(t => t.Name == colorDto.Name);
                if (color == null)
                {
                    colorDto.Id = Guid.NewGuid();
                    await _unitOfWork.Color.AddAsync(colorDto);
                    await _unitOfWork.SaveAsync();
                    colors.Add(colorDto);
                }
                else
                {
                    colors.Add(color);
                }
            }
            return colors;
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

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invaild Model!");
                }

                var product = await _unitOfWork.Product.GetAsync(c => c.Id == productDto.Id);
                if (product == null)
                {
                    return NotFound();
                }

                // Delete the old images if there is an update then add update ProductImages field
                if (product.ProductImages != productDto.ProductImages && !productDto.ProductImages.IsNullOrEmpty())
                {
                    foreach (var productImg in product.ProductImages)
                    {
                        if (!productDto.ProductImages.Exists(x => x.Id == productImg.Id))
                        {
                            DeleteImage(productImg.ImgPath);
                        }
                    }
                    product.ProductImages = productDto.ProductImages;
                }

                //Update tags if there is new tags
                if (product.Tags != productDto.Tags)
                {
                    product.Tags = await UpdateProductTagsAsync(productDto.Tags);
                }
                else
                {
                    product.Tags = productDto.Tags;
                }

                // Update colors if there is new colors
                if (product.Colors != productDto.Colors)
                {
                    product.Colors = await UpdateProductColorsAsync(productDto.Colors);
                }
                else
                {
                    product.Colors = productDto.Colors;
                }

                // Update other fields
                product.Name = productDto.Name;
                product.Description = productDto.Description;
                product.ProductCode = productDto.ProductCode;
                product.ProductSKU = productDto.ProductSKU;
                product.Price = productDto.Price;
                product.Status = productDto.Status;
                product.InStock = productDto.InStock;
                product.CategoryId = productDto.CategoryId;
                product.DiscoutId = productDto.DiscoutId;
                product.UpdatedAt = DateTime.Now;
                // Update quantity in the inventory
                Inventory inventory = await _unitOfWork.Inventory.GetAsync(i => i.Id == product.InventoryId);
                if (inventory != null)
                {
                    inventory.Quantity = productDto.Quantity;
                }

                await _unitOfWork.SaveAsync();
                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                var product = await _unitOfWork.Product.GetAsync(c => c.Id == id && c.IsDeleted != true);
                if (product == null)
                {
                    return NotFound("The product is not exist");
                }
                // Delete the image first
                //DeleteImage(product.ImgPath);
                // Delete the product
                product.IsDeleted = true;
                product.DeletedAt = DateTime.Now;
                //await _unitOfWork.Product.DeleteAsync(product);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductRange([FromBody] ProductDto[] productDtos)
        {
            try
            {
                foreach (var productDto in productDtos)
                {
                    var product = await _unitOfWork.Product.GetAsync(c => c.Id == productDto.Id && c.IsDeleted != true);
                    if (product == null)
                    {
                        return NotFound("The product is not exist");
                    }
                    // Delete the image first
                    //DeleteImage(product.ImgPath);
                    // Delete the product
                    product.IsDeleted = true;
                    product.DeletedAt = DateTime.Now;
                }

                //await _unitOfWork.Product.DeleteRangeAsync();
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
