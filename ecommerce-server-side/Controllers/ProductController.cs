using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DataTransferObjects.Shared;
using Models.Models;
using Utility.ManageFiles;

namespace ecommerce_server_side.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IManageFiles _manageFiles;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, IManageFiles manageFiles)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _manageFiles = manageFiles;

        }


        // <-------Product Actions------->
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            try
            {
                var product = await _unitOfWork.Product.GetAsync(p =>
                (p.Id == id && p.IsDeleted != true), "ProductImages,Tags,Colors,Sizes");
                if (product == null)
                {
                    return NotFound();
                }
                var productResult = _mapper.Map<ProductDto>(product);
                // Get discount
                if (productResult.DiscoutId != null)
                {
                    productResult.Discount = await _unitOfWork.Discount.GetAsync(x => x.Id == productResult.DiscoutId);
                }
                // Get quantity
                var invntory = await _unitOfWork.Inventory.GetAsync(x => x.Id == productResult.InventoryId);

                if (invntory != null)
                {
                    productResult.Quantity = invntory.Quantity;
                }
                return Ok(productResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            try
            {
                var products = await _unitOfWork.Product.GetListAsync(p => p.IsDeleted != true, "ProductImages,Tags,Colors,Sizes,Category,Inventory");
                var productsResult = _mapper.Map<IEnumerable<ProductDto>>(products);

                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> AddProduct([FromBody] ProductDto? productDto)
        {
            try
            {
                if (productDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Product is invalid.");
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
                product.Sizes = await UpdateProductSizesAsync(productDto.Sizes);
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

        // Upload product images
        [HttpPost, DisableRequestSizeLimit]
        [Authorize(Roles = "Administrator")]
        [Route("upload-files")]
        public async Task<IActionResult> UploadProductImages()
        {
            try
            {
                var formCollection = await Request.ReadFormAsync();
                var files = formCollection.Files;
                if (files.Any(f => f.Length == 0))
                {
                    return BadRequest();
                }
                var dbPathList = _manageFiles.UploadFiles(files, "Product");
                return Ok(new { dbPathList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
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
        // If the size is new add it to the db then add to the size list finally return this list.

        private async Task<List<Size>> UpdateProductSizesAsync(List<Size>? sizesDto)
        {
            List<Size> sizes = new List<Size>();
            foreach (var sizeDto in sizesDto)
            {
                var color = await _unitOfWork.Size.GetAsync(t => t.Name == sizeDto.Name);
                if (color == null)
                {
                    sizeDto.Id = Guid.NewGuid();
                    await _unitOfWork.Size.AddAsync(sizeDto);
                    await _unitOfWork.SaveAsync();
                    sizes.Add(sizeDto);
                }
                else
                {
                    sizes.Add(color);
                }
            }
            return sizes;
        }

        // Delete image with path from DB and Local Storage
        private async Task DeleteImage(string imgPath)
        {
            if (!imgPath.IsNullOrEmpty())
            {
                _manageFiles.DeleteImage(imgPath);
                var imgToBeDeleted = await _unitOfWork.ProductImage.GetAsync(x => x.ImgPath == imgPath);
                if (imgToBeDeleted != null)
                {
                    await _unitOfWork.ProductImage.DeleteAsync(imgToBeDeleted);
                }
            }

        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                if (productDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }

                var product = await _unitOfWork.Product.GetAsync(c => c.Id == productDto.Id, "ProductImages,Tags,Colors,Sizes");
                if (product == null)
                {
                    return NotFound();
                }

                // Delete the old images if there is an update then add update ProductImages.
                if (product.ProductImages != productDto.ProductImages && !productDto.ProductImages.IsNullOrEmpty())
                {
                    // Delete image with path from DB and Local Storage
                    foreach (var productImg in product.ProductImages)
                    {
                        await DeleteImage(productImg.ImgPath);
                    }
                    // Add new images in the db
                    foreach (var img in productDto.ProductImages)
                    {
                        ProductImage newImg = new ProductImage()
                        {
                            Id = Guid.NewGuid(),
                            ImgPath = img.ImgPath,
                            ProductId = productDto.Id
                        };
                        await _unitOfWork.ProductImage.AddAsync(newImg);
                    }
                }
                //Update tags if there is new tags
                product.Tags = await UpdateProductTagsAsync(productDto.Tags);
                // Update colors if there is new colors
                product.Colors = await UpdateProductColorsAsync(productDto.Colors);
                // Update sizes if there is new sizes
                product.Sizes = await UpdateProductSizesAsync(productDto.Sizes);
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            try
            {
                var product = await _unitOfWork.Product.GetAsync(c =>
                c.Id == id && c.IsDeleted != true, "ProductImages");
                if (product == null)
                {
                    return NotFound("The product is not exist");
                }
                // Delete the images first
                // Delete image with path from DB and Local Storage
                foreach (var productImg in product.ProductImages)
                {
                    await DeleteImage(productImg.ImgPath);
                }
                // Delete the product
                product.DiscoutId = null;
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteProductRange([FromBody] ProductDto[] productDtos)
        {
            try
            {
                foreach (var productDto in productDtos)
                {
                    var product = await _unitOfWork.Product.GetAsync(c =>
                    c.Id == productDto.Id && c.IsDeleted != true, "ProductImages");
                    if (product == null)
                    {
                        return NotFound("The product is not exist");
                    }
                    // Delete the image first
                    // Delete image with path from DB and Local Storage
                    foreach (var productImg in product.ProductImages)
                    {
                        await DeleteImage(productImg.ImgPath);
                    }
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
