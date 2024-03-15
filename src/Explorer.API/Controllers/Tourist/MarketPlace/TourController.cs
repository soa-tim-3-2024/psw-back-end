using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Public;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist.MarketPlace
{
    [Route("api/market-place")]
    public class TourController : BaseApiController
    {
        private readonly ITourService _tourService;
        private readonly IShoppingCartService _shoppingCartService;
        private static readonly HttpClient _sharedClient = new();

        public TourController(ITourService service, IShoppingCartService shoppingCartService)
        {
            _tourService = service;
            _shoppingCartService = shoppingCartService;
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("tours/published")]
        public async Task<ActionResult<List<TourResponseDto>>> GetPublishedTours([FromQuery] int page, [FromQuery] int pageSize)
        {
            var tours = await GetPublishedToursGo(_sharedClient);
            return tours;
        }

        [HttpGet("tours/{tourId:long}")]
        public async Task<ActionResult<TourResponseDto>> GetByIdAsync(long tourId)
        {
            //var result = _tourService.GetById(tourId);
            //return CreateResponse(result);
            var tour = await GetTourGo(_sharedClient, tourId);
            return tour;
        }

        static async Task<TourResponseDto> GetTourGo(HttpClient httpClient, long id)
        {
            var tour = await httpClient.GetFromJsonAsync<TourResponseDto>(
                "http://localhost:8081/tour/" + id);
            return tour;

        }

        [HttpGet("tours/can-be-rated/{tourId:long}")]
        public bool CanTourBeRated(long tourId)
        {
            long userId = extractUserIdFromHttpContext();
            return _tourService.CanTourBeRated(tourId, userId).Value;
        }

        private long extractUserIdFromHttpContext()
        {
            return long.Parse((HttpContext.User.Identity as ClaimsIdentity).FindFirst("id")?.Value);
        }

        [Authorize(Policy = "touristPolicy")]
        [Authorize(Roles = "tourist")]
        [HttpGet("tours/inCart/{id:long}")]
        public async Task<ActionResult<List<TourResponseDto>>> GetToursInCart([FromQuery] int page, [FromQuery] int pageSize, long id)
        {
            var cart = _shoppingCartService.GetByTouristId(id);
            if (cart.Value == null)
            {
                return NotFound();
            }
            var tourIds = cart.Value.OrderItems.Select(order => order.TourId).ToList();
            //var result = _tourService.GetLimitedInfoTours(page, pageSize, tourIds);
            var result = await GetToursByIdGo(_sharedClient, tourIds);
            return result;
        }
        /*[HttpGet("tours/inCart/{id:long}")]
        public ActionResult<PagedResult<LimitedTourViewResponseDto>> GetToursInCart([FromQuery] int page, [FromQuery] int pageSize, long id)
        {
            var cart = _shoppingCartService.GetByTouristId(id);
            if (cart == null)
            {
                return NotFound();
            }
            var tourIds = cart.Value.OrderItems.Select(order => order.TourId).ToList();
            var result = _tourService.GetLimitedInfoTours(page, pageSize, tourIds);
            return CreateResponse(result);
        }*/

        [HttpGet("tours/adventure")]
        public ActionResult<PagedResult<TourResponseDto>> GetPopularAdventureTours([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetAdventureTours(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("tours/family")]
        public ActionResult<PagedResult<TourResponseDto>> GetPopularFamilyTours([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetFamilyTours(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("tours/cruise")]
        public ActionResult<PagedResult<TourResponseDto>> GetPopularCruiseTours([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetCruiseTours(page, pageSize);
            return CreateResponse(result);
        }

        [HttpGet("tours/cultural")]
        public ActionResult<PagedResult<TourResponseDto>> GetPopularCulturalTours([FromQuery] int page, [FromQuery] int pageSize)
        {
            var result = _tourService.GetCulturalTours(page, pageSize);
            return CreateResponse(result);
        }

        static async Task<List<TourResponseDto>> GetPublishedToursGo(HttpClient httpClient)
        {
            var tours = await httpClient.GetFromJsonAsync<List<TourResponseDto>>(
                "http://localhost:8081/tours/published/all");
            return tours;
        }
        static async Task<List<TourResponseDto>> GetToursByIdGo(HttpClient httpClient, List<long> Ids)
        {
            TourIdsDto IdsDto = new TourIdsDto();
            IdsDto.Ids = Ids;
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(IdsDto),
                Encoding.UTF8,
                "application/json");
            
            var response = await httpClient.PostAsync(
                "http://localhost:8081/tours/tours-list",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var tours = await response.Content.ReadFromJsonAsync<List<TourResponseDto>>();
            return tours;
        }

    }
}
