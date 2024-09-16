using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.TourAuthoring;
using Explorer.Tours.Core.Domain.Tours;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;

namespace Explorer.API.Controllers.Tourist.MarketPlace
{
    [Route("api/market-place")]
    public class KeyPointController : BaseApiController
    {
        private readonly IKeyPointService _keyPointService;
        private static readonly HttpClient _sharedClient = new();
        public KeyPointController(IKeyPointService keyPointService)
        {
            _keyPointService = keyPointService;
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("tours/{tourId:long}/key-points")]
        public async Task<ActionResult<List<KeyPointResponseDto>>> GetKeyPoints(long tourId)
        {
            var keyPoints = await GetKeyPointsGo(_sharedClient, tourId);
            return keyPoints;
        }
        [Authorize(Roles = "tourist")]
        [HttpGet("{campaignId:long}/key-points")]
        public ActionResult<KeyPointResponseDto> GetCampaignKeyPoints(long campaignId)
        {
            var result = _keyPointService.GetByCampaignId(campaignId);
            return CreateResponse(result);
        }

        [Authorize(Roles = "author, tourist")]
        [HttpGet("tours/{tourId:long}/firts-key-point")]
        public ActionResult<KeyPointResponseDto> GetToursFirstKeyPoint(long tourId)
        {
            var result = _keyPointService.GetFirstByTourId(tourId);
            return CreateResponse(result);
        }

        static async Task<List<KeyPointResponseDto>> GetKeyPointsGo(HttpClient httpClient, long tourId)
        {
            
            using HttpResponseMessage response = await httpClient.GetAsync(
                "http://host.docker.internal:8083/keyPoints/tour/" + tourId);
            var keyPoints = await response.Content.ReadFromJsonAsync<List<KeyPointResponseDto>>();
            return keyPoints;
        }
    }
}
