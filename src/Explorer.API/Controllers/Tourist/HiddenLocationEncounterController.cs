using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Explorer.Tours.API.Dtos.TouristPosition;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/hidden-location-encounter")]
    public class HiddenLocationEncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;
        private readonly ITouristProgressService _touristProgressService;
        private readonly IHttpClientFactory _factory;
        public HiddenLocationEncounterController(IEncounterService encounterService, ITouristProgressService touristProgressService, IHttpClientFactory factory)
        {
            _encounterService = encounterService;
            _touristProgressService = touristProgressService;
            _factory = factory;
        }

        [HttpGet("{id:long}")]
        public ActionResult<EncounterResponseDto> GetHiddenLocationEncounterById(long id)
        {
            var result = _encounterService.GetHiddenLocationEncounterById(id);
            return CreateResponse(result);
        }

        [HttpPost("{id:long}/complete")]
        public ActionResult<EncounterResponseDto> Complete([FromBody] TouristPositionCreateDto position, long id)
        {
            long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            var result = _encounterService.CompleteHiddenLocationEncounter(userId, id, position.Longitude, position.Latitude);
            return CreateResponse(result);
        }

        [HttpPost("{id:long}/check-range")]
        public bool CheckIfUserInCompletionRange([FromBody] TouristPositionCreateDto position, long id)
        {
            long userId = int.Parse(HttpContext.User.Claims.First(i => i.Type.Equals("id", StringComparison.OrdinalIgnoreCase)).Value);
            return _encounterService.CheckIfUserInCompletionRange(userId, id, position.Longitude, position.Latitude);
        }

        [HttpPost("create")]
        public async Task<ActionResult<HiddenLocationEncounterResponseDto>> Create([FromBody] HiddenLocationEncounterResponseDto hiddenLocationEncounter)
        {
            var client = _factory.CreateClient();
            var encounterResponse = await CreateHiddenLocationEncounterGo(client, hiddenLocationEncounter);
            return encounterResponse;
        }

        static async Task<HiddenLocationEncounterResponseDto> CreateHiddenLocationEncounterGo(HttpClient httpClient, HiddenLocationEncounterResponseDto encounter)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(encounter),
                Encoding.UTF8,
                "application/json");
            Debug.WriteLine(jsonContent.ToString());
            string jsonString = jsonContent.ReadAsStringAsync().Result;
            Debug.WriteLine(jsonString);
            Debug.WriteLine("!!!");
            Debug.WriteLine("!!!");
            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://localhost:8082/hidden/location/encounters",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var encounterResponse = await response.Content.ReadFromJsonAsync<HiddenLocationEncounterResponseDto>();
            return encounterResponse;
        }

    }
}
