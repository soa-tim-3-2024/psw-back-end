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
    [Route("api/author/misc-encounter")]
    public class MiscEncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;
        private readonly IHttpClientFactory _factory;
        public MiscEncounterController(IEncounterService encounterService, IHttpClientFactory factory)
        {
            _encounterService = encounterService;
            _factory = factory;
        }


        [HttpPost("createMisc")]
        public async Task<ActionResult<MiscEncounterResponseDto>> CreateSocialEncounter([FromBody] MiscEncounterResponseDto miscEncounter)
        {
            var client = _factory.CreateClient();
            var encounterResponse = await CreateMiscEncounterGo(client, miscEncounter);
            return encounterResponse;
        }

        static async Task<MiscEncounterResponseDto> CreateMiscEncounterGo(HttpClient httpClient, MiscEncounterResponseDto encounter)
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
                "http://localhost:8082/misc/encounters",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var encounterResponse = await response.Content.ReadFromJsonAsync<MiscEncounterResponseDto>();
            return encounterResponse;
        }

    }
}
