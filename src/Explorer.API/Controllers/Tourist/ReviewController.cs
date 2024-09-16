using System.Security.Claims;
using Castle.Components.DictionaryAdapter.Xml;
using System.Text.Json;
using System.Text;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Explorer.Tours.Core.Domain.Tours;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Explorer.Stakeholders.API.Public;
using Explorer.Tours.Core.Domain;


namespace Explorer.API.Controllers.Tourist
{
    
    [Route("api/review")]
    public class ReviewController : BaseApiController
    {
        private readonly IReviewService _reviewService;
        private readonly IUserService _userService;
        private static readonly HttpClient _sharedClient = new();

        public ReviewController(IReviewService reviewService, IUserService userService)
        {
            _reviewService = reviewService;
            _userService = userService;
        }

        //[Authorize(Policy = "nonAdministratorPolicy")]
        [HttpGet("{tourId:int}")]
        public async Task<ActionResult<List<ReviewResponseDto>>> GetAllByTourId([FromQuery] int page, [FromQuery] int pageSize, long tourId)
        {
            //var result = _reviewService.GetPagedByTourId(page, pageSize, tourId);
            //return CreateResponse(result);
            var tours = await _sharedClient.GetFromJsonAsync<List<ReviewResponseDto>>(
                "http://host.docker.internal:8083/reviews/" + tourId);
            return tours;
        }

        [Authorize(Policy = "nonAdministratorPolicy")]
        [HttpGet("{touristId:long}/{tourId:long}")]
        public ActionResult<Boolean> ReviewExists(long touristId, long tourId)
        {
            var result = _reviewService.ReviewExists(touristId, tourId);
            return result.Value;
        }

        [Authorize(Policy = "touristPolicy")]
        [HttpPost]
        public async Task<ActionResult<ReviewResponseDto>> Create([FromBody] ReviewCreateDto review)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            { 
              review.TouristId = long.Parse(identity.FindFirst("id").Value);  
            }
            review.CommentDate = DateOnly.FromDateTime(DateTime.Now);
            //var result = _reviewService.Create(review);
            //return CreateResponse(result);
            using StringContent jsonContent = new(
               JsonSerializer.Serialize(review),
               Encoding.UTF8,
               "application/json");
            var res = await _sharedClient.PostAsync(
                "http://host.docker.internal:8083/review", jsonContent);
            var resFinal = await res.Content.ReadFromJsonAsync<ReviewResponseDto>();
            return resFinal;
        }

        [Authorize(Policy = "touristPolicy")]
        [HttpPut("{id:int}")]
        public ActionResult<ReviewResponseDto> Update([FromBody] ReviewUpdateDto review)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                if (long.Parse(identity.FindFirst("id").Value) != review.TouristId)
                    return Forbid();
            }
            var result = _reviewService.Update(review);
            return CreateResponse(result);
        }

        [Authorize(Policy = "touristPolicy")]
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var result = _reviewService.Delete(id);
            return CreateResponse(result);
        }

        [HttpGet("usernameById/{userId:int}")]
        public ActionResult<String> GetCurrentUsername(long userId)
        {
            var user = _userService.Get(userId).Value;
            return user.Username;
        }
    }
}
