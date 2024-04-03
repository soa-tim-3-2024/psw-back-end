using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Dtos.TouristPosition;
using Explorer.Tours.API.Public;
using Explorer.Payments.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.API.Dtos;
using FluentResults;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourexecution/tourexecution")]
    public class TourExecutionSessionController : BaseApiController
    {
        private readonly ITourExecutionSessionService _tourExecutionService;
        private readonly ITourService _tourService;
        private readonly ITourTokenService _tourTokenService;
        private static readonly HttpClient _sharedClient = new();
        public TourExecutionSessionController(ITourExecutionSessionService tourExecutionService, ITourService tourService, ITourTokenService tourTokenService)
        {
            _tourExecutionService = tourExecutionService;
            _tourService = tourService;
            _tourTokenService = tourTokenService;
        }

        [HttpGet]
        [Route("purchasedtours")]
        public async Task<ActionResult<List<TourResponseDto>>> GetPurchasedTours()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long touristId;
            touristId = long.Parse(identity.FindFirst("id").Value);
            var purchasedTourIds = _tourTokenService.GetTouristToursId(touristId).Value;
            var result = await GetToursByIdGo(_sharedClient, purchasedTourIds);
            return result;
        }

        [HttpGet("{tourId:long}")]
        public ActionResult<PagedResult<TourResponseDto>> GetById(long tourId)
        {
            var result = _tourService.GetById(tourId);
            return CreateResponse(result);
        }

        [HttpPost]
        public async Task<ActionResult<TourExecutionSessionResponseDto>> StartTour(TourExecutionDto executionDto)
        {
            // treba provera da li je tura kupljena
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long touristId;
            if (identity != null && identity.IsAuthenticated)
                touristId = long.Parse(identity.FindFirst("id").Value);
            // za potrebe testiranja
            else
                touristId = -21;
            if (_tourExecutionService.GetLive(touristId) != null)
            {
                return Conflict();
            }
            var result = await StartTourGo(_sharedClient, executionDto.TourId, touristId);
            return result;
        }

        [HttpPut]
        [Route("abandoning/{executionId}")]
        public async Task<ActionResult<TourExecutionSessionResponseDto>> AbandonTour([FromBody]TourExecutionDto executionDto, long executionId)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long touristId;
            if (identity != null && identity.IsAuthenticated)
                touristId = long.Parse(identity.FindFirst("id").Value);
            // za potrebe testiranja
            else
                touristId = -21;
            var result = await AbandondTourGo(_sharedClient, executionId);
            if (result == null)
            {
                return BadRequest();
            }
            return result;
        }

        [HttpPut]
        [Route("{tourId:long}/{isCampaign:bool}/keypoint")]
        public async Task<ActionResult<TourExecutionSessionResponseDto>> CompleteKeyPoint(long tourId, bool isCampaign, TouristPositionResponseDto touristPosition)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long touristId;
            if (identity != null && identity.IsAuthenticated)
                touristId = long.Parse(identity.FindFirst("id").Value);
            // za potrebe testiranja
            else
                touristId = -21;
            var result = await CheckCompletititonGo(_sharedClient, tourId, touristId, touristPosition.Longitude, touristPosition.Latitude);
            if (result == null)
            {
                return BadRequest();
            }
            return result;
        }
        [HttpGet]
        [Route("allInfo")]
        public ActionResult<PagedResult<TourExecutionInfoDto>> GetExecutedToursInfo()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long touristId;
            if (identity != null && identity.IsAuthenticated)
                touristId = long.Parse(identity.FindFirst("id").Value);
            // za potrebe testiranja
            else
                touristId = -21;
            var result = _tourExecutionService.GetAllFor(touristId);
            return CreateResponse(result);
        }
        [HttpGet]
        [Route("live")]
        public ActionResult<PagedResult<TourExecutionSessionResponseDto>> GetLiveTour()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            long touristId = long.Parse(identity.FindFirst("id").Value);
            var result = _tourExecutionService.GetLive(touristId);
            if (result == null)
            {
                return NoContent();
            }
            return CreateResponse(result);
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
                "http://host.docker.internal:8081/tours/tours-list",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var tours = await response.Content.ReadFromJsonAsync<List<TourResponseDto>>();
            return tours;
        }

        static async Task<TourExecutionSessionResponseDto> StartTourGo(HttpClient httpClient, long tourId, long touristId)
        {
            var response = await httpClient.GetAsync(
                "http://host.docker.internal:8081/tour-execution/" + tourId+"/"+touristId);
            var execution = await response.Content.ReadFromJsonAsync<TourExecutionSessionResponseDto>();
            return execution;
        }

        static async Task<TourExecutionSessionResponseDto> AbandondTourGo(HttpClient httpClient, long id)
        {
            var response = await httpClient.GetAsync(
                "http://host.docker.internal:8081/tour-execution-abandoning/" + id);
            var execution = await response.Content.ReadFromJsonAsync<TourExecutionSessionResponseDto>();
            return execution;
        }

        static async Task<TourExecutionSessionResponseDto> CheckCompletititonGo(HttpClient httpClient, long tourId, long touristId, double longitude, double latitude)
        {
            CheckCompletitionDto completitionDto = new CheckCompletitionDto();
            completitionDto.TourId = tourId;
            completitionDto.TouristId = touristId;
            completitionDto.Longitude = longitude;
            completitionDto.Latitude = latitude;

            using StringContent jsonContent = new(
                JsonSerializer.Serialize(completitionDto),
                Encoding.UTF8,
                "application/json");

            var response = await httpClient.PostAsync(
                "http://host.docker.internal:8081/tour-execution/check-completition",
                jsonContent);
            var execution = await response.Content.ReadFromJsonAsync<TourExecutionSessionResponseDto>();
            return execution;
        }

    }
}
