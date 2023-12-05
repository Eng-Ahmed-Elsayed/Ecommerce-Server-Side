using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Customer;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/users/checkList")]
    [Authorize]
    [ApiController]
    public class CheckListController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CheckListController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        // The user get his checkList
        // If there is no checkList create one
        // If any checkListItem is an deleted product delete it from the checkList
        [HttpGet]
        public async Task<IActionResult> CheckListReview()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var checkList = await _unitOfWork.CheckList.GetAsync(x =>
                    x.UserId == userId && x.IsDeleted != true
                    , "CheckListItems.Product.ProductImages,CheckListItems.Product.Inventory,CheckListItems.Product.Category");

                    // If the user does not have shopping cart create one.
                    if (checkList == null)
                    {
                        checkList = await CreateCheckList(userId);
                        if (checkList == null)
                        {
                            return BadRequest("Can't create a shopping cart.");
                        }
                    }
                    await DeleteNotAvailableCheckListItems(checkList);
                    var checkListItemsResult = _mapper.Map<IEnumerable<CheckListItemDto>>(checkList.CheckListItems)
                        .Select(c =>
                        {
                            c.Product.IsInCheckList = true;
                            return c;
                        });

                    return Ok(checkListItemsResult);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }

        }

        // Create New User CheckList.
        private async Task<CheckList> CreateCheckList(string userId)
        {

            CheckList checkList = new CheckList()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CreatedAt = DateTime.Now,
            };
            var result = await _unitOfWork.CheckList.AddAsync(checkList);
            if (result)
            {
                await _unitOfWork.SaveAsync();
                return checkList;
            }
            return null;

        }

        // Check if the checkList item and the product is available first
        // and if not delete it from the checkList.
        private async Task DeleteNotAvailableCheckListItems(CheckList checkList)
        {
            checkList.CheckListItems.RemoveAll(x =>
                                            x.IsDeleted == true
                                            || x.Product.IsDeleted == true);
            await _unitOfWork.SaveAsync();
        }


        // Add new checkListItem if it is an vaild product
        [HttpPost]
        public async Task<IActionResult> AddToCheckList([FromBody] CheckListItemDto checkListItemDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var checkList = await _unitOfWork.CheckList.GetAsync(x =>
                    x.UserId == userId && x.IsDeleted != true,
                    "CheckListItems");
                    // If the user does not have checkList create one.
                    if (checkList == null)
                    {
                        checkList = await CreateCheckList(userId);
                        if (checkList == null)
                        {
                            return BadRequest("Can't create a user checkList.");
                        }
                    }
                    // Check if this product is not deleted.
                    Product product = await _unitOfWork.Product.GetAsync(x =>
                    x.Id == checkListItemDto.ProductId && x.IsDeleted != true);
                    if (product == null)
                    {
                        return NotFound("Product is not available.");
                    }
                    // Check if the product is already in the checkList.
                    if (checkList.CheckListItems.Any(x => x.ProductId == product.Id
                    && x.IsDeleted != true))
                    {
                        return BadRequest("This produt is already in your checkList.");
                    }
                    // Add new checkList item
                    checkListItemDto.Id = Guid.NewGuid();
                    var checkListItem = _mapper.Map<CheckListItem>(checkListItemDto);
                    checkListItem.CheckListId = checkList.Id;
                    checkListItem.CreatedAt = DateTime.Now;
                    var result = await _unitOfWork.CheckListItem.AddAsync(checkListItem);
                    if (result)
                    {
                        await _unitOfWork.SaveAsync();
                        return Ok(checkListItemDto);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        // Delete a checkListItem if it's already exists.
        [Route("{id:Guid}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCheckListItem([FromRoute] Guid id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userId = User.FindFirst("id")?.Value;
                    var checkList = await _unitOfWork.CheckList.GetAsync(x =>
                                        x.UserId == userId
                                        && x.IsDeleted != true);

                    var checkListItem = await _unitOfWork.CheckListItem.GetAsync(x =>
                                    x.ProductId == id
                                    && x.CheckListId == checkList.Id
                                    && x.IsDeleted != true);

                    if (checkListItem != null)
                    {
                        checkListItem.IsDeleted = true;
                        checkListItem.DeletedAt = DateTime.Now;
                        await _unitOfWork.SaveAsync();
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }
    }
}
