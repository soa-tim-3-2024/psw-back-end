using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Explorer.API.Controllers
{
    [Authorize(Policy = "nonAdministratorPolicy")]
    [Route("api/follower")]
    public class FollowerController : BaseApiController
    {
        private readonly IFollowerService _followerService;
        private readonly IUserService _userService;
        private static readonly HttpClient _sharedClient = new();
        public FollowerController(IFollowerService followerService, IUserService userService)
        {
            _followerService = followerService;
            _userService = userService;
        }
        /*
        [HttpGet("followers/{id:long}")]
        public ActionResult<PagedResult<FollowerResponseWithUserDto>> GetFollowers([FromQuery] int page, [FromQuery] int pageSize, long id)
        {
            long userId = id;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                userId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _followerService.GetFollowers(page, pageSize, userId);
            return CreateResponse(result);
        }
        [HttpGet("followings/{id:long}")]
        public ActionResult<PagedResult<FollowingResponseWithUserDto>> GetFollowings([FromQuery] int page, [FromQuery] int pageSize, long id)
        {
            long userId = id;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                userId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _followerService.GetFollowings(page, pageSize, userId);
            return CreateResponse(result);
        }

        //[HttpGet("user-followings/{id}")]
        public async Task<ActionResult<List<FollowingResponseDto>>> GetUserFollowings(string id)
        {
            var followings = await _sharedClient.GetFromJsonAsync<FollowingResponseDto[]>(
                "http://host.docker.internal:8089/user-followings/" + id);
            return followings.ToList();
        }
        */
        //[HttpGet("user-followers/{id}")]
        /*
        public async Task<ActionResult<List<FollowingResponseDto>>> GetUserFollowers(string id)
        {
            var followers = await _sharedClient.GetFromJsonAsync<FollowingResponseDto[]>(
                "http://host.docker.internal:8089/user-followers/" + id);
            return followers.ToList();
        }

        //[HttpGet("user-recommendations/{id}")]
        public async Task<ActionResult<List<FollowingResponseDto>>> GetUserRecommendations(string id)
        {
            var followers = await _sharedClient.GetFromJsonAsync<FollowingResponseDto[]>(
                "http://host.docker.internal:8089/user-recommendations/" + id);
            return followers.ToList();
        }

        *//*
        [HttpDelete("{id:long}")]
        public ActionResult Delete(long id)
        {
            var result = _followerService.Delete(id);
            return CreateResponse(result);
        }

        [HttpPost]
        public ActionResult<FollowerResponseDto> Create([FromBody] FollowerCreateDto follower)
        {
            var result = _followerService.Create(follower);
            return CreateResponse(result);
        }

        //[HttpPost("create")]
        public async Task<ActionResult<FollowerResponseDto>> CreateNewFollowing([FromBody] NewFollowingDto following)
        {
            var res = await CreateFollowingGo(_sharedClient, following);
            return res;
        }

        //[HttpPut("unfollow")]
        public async Task<ActionResult<FollowerResponseDto>> UnfollowUser([FromBody] UnfollowUserDto unfollow)
        {
            var res = await UnfollowUserGo(_sharedClient, unfollow);
            return res;
        }

        [HttpGet("search/{searchUsername}")]
        public ActionResult<PagedResult<UserResponseDto>> GetSearch([FromQuery] int page, [FromQuery] int pageSize, string searchUsername)
        {
            long userId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                userId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _userService.SearchUsers(0, 0, searchUsername, userId);
            return CreateResponse(result);
        }
        static async Task<FollowerResponseDto> CreateFollowingGo(HttpClient httpClient, NewFollowingDto following)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(following),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PostAsync(
                "http://host.docker.internal:8089/following",
                jsonContent);

            var res = await response.Content.ReadFromJsonAsync<FollowerResponseDto>();
            return res;
        }
        static async Task<FollowerResponseDto> UnfollowUserGo(HttpClient httpClient, UnfollowUserDto unfollow)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(unfollow),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine(jsonContent);

            using HttpResponseMessage response = await httpClient.PutAsync(
                "http://host.docker.internal:8089/unfollow",
                jsonContent);

            var res = await response.Content.ReadFromJsonAsync<FollowerResponseDto>();
            return res;
        }
        */
        [HttpGet("search/{searchUsername}")]
        public ActionResult<PagedResult<UserResponseDto>> GetSearch([FromQuery] int page, [FromQuery] int pageSize, string searchUsername)
        {
            long userId = 0;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                userId = long.Parse(identity.FindFirst("id").Value);
            }
            var result = _userService.SearchUsers(0, 0, searchUsername, userId);
            return CreateResponse(result);
        }
    }

}
