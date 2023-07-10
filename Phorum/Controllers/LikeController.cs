using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phorum.Entities;
using Phorum.Services;
using System.Security.Claims;

namespace Phorum.Controllers
{
    [ApiController]
    [Route("api/phorum/like")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILogger<PostsController> _logger;
        private readonly ILikeService _likeService;

        public LikeController(ILogger<PostsController> logger, IMapper autoMapper, ILikeService likeService)
        {
            _logger = logger;
            _likeService = likeService;
        }

        [HttpPost]
        public ActionResult Like(int postId)
        {
            _likeService.LikePost(postId);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete(int postId)
        {
            _likeService.DeleteLike(postId);
            return Ok();
        }
    }
}