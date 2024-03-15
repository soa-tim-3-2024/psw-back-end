using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Author
{
    [Authorize(Policy = "authorPolicy")]
    [Route("api/author/equipment")]
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
            var pref = await _sharedClient.GetFromJsonAsync<List<EquipmentResponseDto>>(
                "http://localhost:8081/equipment/all");
            return pref;
        }
    }
}
