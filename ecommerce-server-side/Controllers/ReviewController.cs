using System.Security.Claims;
using AutoMapper;
using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DataTransferObjects.Auth;
using Models.DataTransferObjects.Shared;
using Models.Models;

namespace ecommerce_server_side.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReviewController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetReview([FromRoute] Guid id)
        {
            try
            {
                // If review and product are not deleted.
                var review = await _unitOfWork.Review.GetAsync(r => r.Id == id
                                && r.IsDeleted != true
                                && r.Product.IsDeleted != true,
                                "Product");
                if (review == null)
                {
                    return NotFound();
                }
                var reviewResult = _mapper.Map<ReviewDto>(review);
                return Ok(reviewResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ReviewList()
        {
            try
            {
                var userId = User.FindFirst("id")?.Value;
                // If review and product are not deleted.
                var reviews = await _unitOfWork.Review.GetListAsync(r => r.IsDeleted != true
                                && r.UserId == userId
                                && r.Product.IsDeleted != true,
                                "Product");
                var reviewsResult = _mapper.Map<IEnumerable<ReviewDto>>(reviews);
                return Ok(reviewsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewDto? reviewDto)
        {
            try
            {
                if (reviewDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Review is invalid.");
                }
                var userId = User.FindFirst("id")?.Value;
                // Check if there is a product with this id and the user does not have a review in this product.
                var product = await _unitOfWork.Product.GetAsync(p => p.Id == reviewDto.ProductId
                                && p.IsDeleted != true
                                && !p.Reviews.Any(x => x.UserId == userId && x.IsDeleted != true)
                                );
                if (product == null)
                {
                    return NotFound("Cann't find the product.");
                }
                reviewDto.Id = Guid.NewGuid();
                reviewDto.Title = reviewDto.Title.ToUpper();
                reviewDto.UserId = userId;
                reviewDto.CreatedAt = DateTime.Now;
                Review review = _mapper.Map<Review>(reviewDto);
                bool result = await _unitOfWork.Review.AddAsync(review);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                    UserDto user = new() { Id = userId, UserName = User.FindFirst(ClaimTypes.NameIdentifier)?.Value };
                    reviewDto.User = user;
                    reviewDto.UserId = userId;
                    return Ok(reviewDto);
                }
                return BadRequest("Something wrong happen, please try later.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewDto reviewDto)
        {
            try
            {
                if (reviewDto == null || !ModelState.IsValid)
                {
                    return BadRequest("Invalid Model!");
                }
                var userId = User.FindFirst("id")?.Value;
                // If review and product are not deleted.
                var review = await _unitOfWork.Review.GetAsync(r => r.Id == reviewDto.Id
                                && r.UserId == userId
                                && r.IsDeleted != true
                                && r.Product.IsDeleted != true,
                                "Product");
                if (review == null)
                {
                    return NotFound();
                }

                // Update fields
                review.Title = reviewDto.Title.ToUpper();
                review.Comment = reviewDto.Comment;
                review.Rating = reviewDto.Rating;
                review.UpdatedAt = DateTime.Now;
                await _unitOfWork.SaveAsync();

                reviewDto.UserId = userId;
                return Ok(reviewDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an unexpected condition. Please try again later.");
            }
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteReview([FromRoute] Guid id)
        {
            try
            {
                // If review and product are not deleted.
                var review = await _unitOfWork.Review.GetAsync(r => r.Id == id
                                && r.IsDeleted != true
                                && r.Product.IsDeleted != true,
                                "Product");
                if (review == null)
                {
                    return NotFound("The review does not exist");
                }

                // Delete the review
                review.IsDeleted = true;
                review.DeletedAt = DateTime.Now;
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
