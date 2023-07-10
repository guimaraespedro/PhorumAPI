using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phorum.Entities;
using Phorum.Models;
using Phorum.Services;
using System.Security.Claims;

namespace Phorum.Controllers
{
    [ApiController]
    [Route("api/phorum/posts")]
    [Authorize]
    public class PostsController : ControllerBase
    {

        private readonly ILogger<PostsController> _logger;
        private readonly PhorumContext _phorumContext;
        private readonly IMapper _autoMapper;
        private readonly IPostService _postService;

        public PostsController(ILogger<PostsController> logger, PhorumContext phorumContext, IMapper autoMapper, IPostService postService)
        {
            _logger = logger;
            _phorumContext = phorumContext;
            _autoMapper = autoMapper;
            _postService = postService;
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreatePostDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _postService.CreatePost(model.Content);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<ICollection<PostDTO>> Get()
        {
            ICollection<PostDTO> posts = _postService.GetAllPosts();

            return Ok(posts);
        }

        [HttpGet("{postId}")]
        [AllowAnonymous]
        public ActionResult<PostDTO> Get(int postId)
        {
            PostDTO? post = _postService.GetPostById(postId);

            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPut]
        public ActionResult Put([FromBody] PostDTO model, int postId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _postService.UpdatePost(model, postId);

            return Ok();
        }
    }
}