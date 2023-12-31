﻿using System.Security.Claims;
using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models.DataQuerying;
using Models.DataTransferObjects.Shared;
using Models.Models;
using Newtonsoft.Json;
using Utility.ManageFiles;

namespace ecommerce_server_side.Controllers
{
    [Route("api/products")]
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
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                Product? product;
                string includeProperites = "ProductImages,Tags,Colors,Sizes,Category,Inventory,Reviews.User";
                //if admin
                if (userRole == "Administrator")
                {
                    product = await _unitOfWork.Product.GetAsync(p =>
                            p.Id == id
                            && p.IsDeleted != true
                            , includeProperites);
                }
                // else status is publish
                else
                {
                    product = await _unitOfWork.Product.GetAsync(p =>
                            p.Id == id
                            && p.IsDeleted != true
                            && p.Status == "publish"
                            , includeProperites);
                }

                if (product == null)
                {
                    return NotFound();
                }
                // Filter reviews.
                product.Reviews.RemoveAll(r => r.IsDeleted == true);
                var productResult = _mapper.Map<ProductDto>(product);

                // If the user is logged in.
                if (!userRole.IsNullOrEmpty())
                {
                    var userId = User.FindFirst("id")?.Value;
                    var checkList = await _unitOfWork.CheckList.GetAsync(x =>
                    x.UserId == userId && x.IsDeleted != true,
                    "CheckListItems");
                    // If the user has checkList.
                    // else we do not need to do anything because his checkList is empty
                    if (checkList != null)
                    {
                        // If this porudct in his checkListItems mark it.
                        if (checkList.CheckListItems.Any(i => i.ProductId == productResult.Id
                            && i.IsDeleted != true))
                        {
                            productResult.IsInCheckList = true;
                        }
                    }
                }
                // Filter reviews.
                //productResult.Reviews = productResult.Reviews.FindAll(r => r.IsDeleted != true);
                return Ok(productResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductList()
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                IEnumerable<Product> products;
                string includeProperites = "ProductImages,Tags,Colors,Sizes,Inventory,Reviews,Category";
                //if admin
                if (userRole == "Administrator")
                {
                    products = await _unitOfWork.Product.GetListAsync(p => p.IsDeleted != true
                       , includeProperites);
                }
                // else status is publish
                else
                {
                    products = await _unitOfWork.Product.GetListAsync(p =>
                                p.IsDeleted != true
                                && p.Status == "publish"
                                    , includeProperites);
                }

                var productsResult = _mapper.Map<IEnumerable<ProductDto>>(products);
                // If the user is logged in.
                if (!userRole.IsNullOrEmpty())
                {
                    var userId = User.FindFirst("id")?.Value;
                    var checkList = await _unitOfWork.CheckList.GetAsync(x =>
                    x.UserId == userId && x.IsDeleted != true,
                    "CheckListItems");
                    // If the user has checkList.
                    // else we do not need to do anything because his checkList is empty
                    if (checkList != null)
                    {
                        productsResult = productsResult.Select(x =>
                        {
                            // If this porudct in his checkListItems mark it.
                            if (checkList.CheckListItems.Any(i => i.ProductId == x.Id
                                && i.IsDeleted != true))
                            {
                                x.IsInCheckList = true;
                            }
                            return x;
                        });
                    }

                }
                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }
        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> SearchAndFilterProducts([FromQuery] ProductParameters productParameters)
        {
            try
            {
                var products = await _unitOfWork.Product.GetPagedListAsync(productParameters,
                            "ProductImages,Tags,Colors,Sizes,Category,Reviews,Inventory");

                var metadata = new
                {
                    products.TotalCount,
                    products.PageSize,
                    products.CurrentPage,
                    products.TotalPages,
                    products.HasNext,
                    products.HasPrevious
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                Response.Headers.Add("Access-Control-Expose-Headers", "X-Pagination");
                var productsResult = _mapper.Map<IEnumerable<ProductDto>>(products);
                return Ok(productsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
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
                product.ProductImages = productDto.ProductImages.Select(x => new ProductImage
                {
                    Id = Guid.NewGuid(),
                    ImgPath = x.ImgPath,
                    ProductId = x.ProductId
                }).ToList();
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
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
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
                return StatusCode(500, "The server encountered an unexpected condition. Please try again later.");
            }
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

        // For Tags, Colors and Sizes we can use normal way to update them but i did not create
        // a controller or actions for creating or updating them so what we will apply here
        // for example if color red is not in the database creat it then add it to the product
        // if it is in the database just add this to the product and so on.
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

        // Update product
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

                var product = await _unitOfWork.Product.GetAsync(c => c.Id == productDto.Id
                                && c.IsDeleted != true,
                                "ProductImages,Tags,Colors,Sizes,Reviews");
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
                //product.InStock = productDto.InStock;
                product.CategoryId = productDto.CategoryId;
                product.Featured = productDto.Featured;
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
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
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
                product.Discounts.RemoveAll(x => true);
                product.IsDeleted = true;
                product.DeletedAt = DateTime.Now;
                //await _unitOfWork.Product.DeleteAsync(product);
                await _unitOfWork.SaveAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
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
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        // Get feature and not featrue products to the admin.
        [HttpGet]
        [Route("feature-products")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetFeatureProductsAndOther()
        {
            try
            {
                // Get all products
                var products = await _unitOfWork.Product.GetListAsync(p => p.IsDeleted != true
                       , "ProductImages,Category");
                if (products == null)
                {
                    return BadRequest();
                }
                // Create new obj
                UpsertFeatureProductsDto upsertFeatureProductsDto = new();
                // Set feature products
                upsertFeatureProductsDto.FeatureProducts = _mapper.Map<IEnumerable<ProductDto>>
                    (products.Where(p => p.Featured == true));
                // Set other products
                upsertFeatureProductsDto.OtherProducts = _mapper.Map<IEnumerable<ProductDto>>
                    (products.Where(p => p.Featured != true));
                return Ok(upsertFeatureProductsDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        // Add product to feature products or remove it.
        [HttpPatch]
        [Route("feature-products")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpsertFeatureProducts(UpsertFeatureProductsDto upsertFeatureProductsDto)
        {
            try
            {
                // Get all products
                var products = await _unitOfWork.Product.GetListAsync(p => p.IsDeleted != true
                       , "ProductImages");
                if (products == null)
                {
                    return BadRequest();
                }
                // Set old feature products
                var oldFeatureProducts = products.Where(p => p.Featured == true);
                // Set old other products
                var oldOtherProducts = products.Where(p => p.Featured != true);
                // Loop in new feature products
                // If feature product also in old list containue
                foreach (var product in upsertFeatureProductsDto.FeatureProducts)
                {
                    // If new feature product not in the old list make this product with true
                    if (!oldFeatureProducts.Any(p => p.Id == product.Id))
                    {
                        // Get the product
                        var updatedProduct = await _unitOfWork.Product.GetAsync(p => p.Id == product.Id
                        && p.IsDeleted != true);
                        // Mark it true
                        updatedProduct.Featured = true;
                    }
                }
                // Loop in old feature products
                foreach (var product in oldFeatureProducts)
                {
                    // If old feature product not in the new list mark this porduct with false
                    if (!upsertFeatureProductsDto.FeatureProducts.Any(p => p.Id == product.Id))
                    {
                        // Mark it with false
                        product.Featured = false;
                    }
                }
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
