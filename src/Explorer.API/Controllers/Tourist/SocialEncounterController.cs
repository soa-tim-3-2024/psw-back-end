using Explorer.Encounters.API.Dtos;
using Explorer.Encounters.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Explorer.API.Controllers.Tourist
{
    [Authorize(Policy = "touristPolicy")]
    [Route("api/tourist/social-encounter")]
    public class SocialEncounterController : BaseApiController
    {
        private readonly IEncounterService _encounterService;
        private readonly ITouristProgressService _touristProgressService;
        private readonly IHttpClientFactory _factory;
        public SocialEncounterController(IEncounterService encounterService, ITouristProgressService touristProgressService, IHttpClientFactory factory)
        {
            _encounterService = encounterService;
            _touristProgressService = touristProgressService;
            _factory = factory;
        }

        [HttpPost("create")]
        public async Task<ActionResult<SocialEncounterCreateDto>> CreateSocialEncounter([FromBody] SocialEncounterCreateDto socialEncounter)
        {
            var client = _factory.CreateClient();
            var encounterResponse = await CreateSocialEncounterGo(client, socialEncounter);
            return encounterResponse;
        }

        static async Task<SocialEncounterCreateDto> CreateSocialEncounterGo(HttpClient httpClient, SocialEncounterCreateDto encounter)
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
                "http://host.docker.internal:8082/social/encounters",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var encounterResponse = await response.Content.ReadFromJsonAsync<SocialEncounterCreateDto>();
            return encounterResponse;
        }
    }
}
