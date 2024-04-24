using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
using System.Text;
using System.Net.Http;

namespace Explorer.API.Controllers.Administrator.Administration
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/equipment")]
    public class EquipmentController : BaseApiController
    {
        private readonly IEquipmentService _equipmentService;
        private static readonly HttpClient _sharedClient = new();

        public EquipmentController(IEquipmentService equipmentService)
        {
            _equipmentService = equipmentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EquipmentResponseDto>>> GetAll([FromQuery] int page, [FromQuery] int pageSize)
        {
            //var result = _equipmentService.GetPaged(page, pageSize);
            //return CreateResponse(result);
            var eqs = await _sharedClient.GetFromJsonAsync<List<EquipmentResponseDto>>(
                "http://host.docker.internal:8083/equipment/all");
            return eqs;
        }

        [HttpPost]
        public async Task<ActionResult<EquipmentResponseDto>> Create([FromBody] EquipmentCreateDto equipment)
        {
            //var result = _equipmentService.Create(equipment);
            //return CreateResponse(result);
            var tourResponse = await CreateGo(_sharedClient, equipment);
            return tourResponse;
        }

        static async Task<EquipmentResponseDto> CreateGo(HttpClient httpClient, EquipmentCreateDto eq)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(eq),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://host.docker.internal:8083/equipment",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var eqResponse = await response.Content.ReadFromJsonAsync<EquipmentResponseDto>();
            return eqResponse;
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<EquipmentResponseDto>> Update([FromBody] EquipmentUpdateDto equipment)
        {
            //var result = _equipmentService.Update(equipment);
            //return CreateResponse(result);

            var tourResponse = await UpdateGo(_sharedClient, equipment);
            return tourResponse;
        }

        static async Task<EquipmentResponseDto> UpdateGo(HttpClient httpClient, EquipmentUpdateDto equipment)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(equipment),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PutAsync(
                "http://host.docker.internal:8083/equipment",
                jsonContent);
            Debug.WriteLine(jsonContent.ReadAsStringAsync().Result);
            var eqResponse = await response.Content.ReadFromJsonAsync<EquipmentResponseDto>();
            return eqResponse;
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            //var result = _equipmentService.Delete(id);
            //return CreateResponse(result);
            var response = await _sharedClient.DeleteAsync(
                "http://host.docker.internal:8083/equipment/" + id);
            return Ok(response.Content);
        }
    }
}
