using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/hidden-location-encounter")]
    public class HiddenLocationEncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;
        private readonly IHttpClientFactory _factory;
        public HiddenLocationEncounterController(IEncounterService encounterService, IHttpClientFactory factory)
        {
            _encounterService = encounterService;
            _factory = factory;
        }

        [HttpPost("create")]
        public async Task<ActionResult<HiddenLocationEncounterResponseDto>> CreateSocialEncounter([FromBody] HiddenLocationEncounterResponseDto hiddenLocationEncounter)
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
