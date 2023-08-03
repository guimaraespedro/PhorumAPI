using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phorum.Entities;
using Phorum.Helpers;
using Phorum.Models;
using Phorum.Repositories.PostRepository;
using System.Security.Claims;

namespace Phorum.Services
{
    public class PostService : IPostService
    {
        private readonly PhorumContext _phorumContext;
        private readonly IMapper _mapper;
        private readonly HttpContextHelper _httpContextHelper;
        private readonly IPostRepository _postRepository;
        public PostService(PhorumContext context, IMapper mapper, HttpContextHelper httpContextHelper, IPostRepository postRepository) {
            _phorumContext = context;
            _mapper = mapper;
            _httpContextHelper = httpContextHelper;
            _postRepository = postRepository;
        }

        public void CreatePost(string content)
        {

            DateTime now = DateTime.Now;

            Post post = new()
            {
                Content = content,
                CreatedAt = now,
                UpdatedAt = now,
            };

            int userId = _httpContextHelper.GetUserId();
            post.UserId = userId;
            User? user = _phorumContext.User.Find(userId);

            ArgumentNullException.ThrowIfNull(user);

            post.User = user;

            _postRepository.CreatePost(post);
            _postRepository.SaveChanges();
        }

        public ICollection<PostDTO> GetAllPosts()
        {
            ICollection<PostDTO> posts =_postRepository.GetAllPosts()
               .Select(post => new PostDTO()
               {
                   Id = post.Id,
                   Content = post.Content,
                   CreatedAt = post.CreatedAt,
                   User = new()
                   {
                       Id = post.User.Id,
                       Name = post.User.Name
                   }
               }).OrderByDescending(post=> post.CreatedAt).ToList();

            return posts;
        }

        public PostDTO GetPostById(int postId)
        {
            Post? post = _postRepository.GetPostById(postId);

            ArgumentNullException.ThrowIfNull(post);

            PostDTO postDTO = _mapper.Map<PostDTO>(post);

            return postDTO;
        }


        public void UpdatePost(PostDTO model, int postId)
        {
            int userId = _httpContextHelper.GetUserId();

            Post? post = _phorumContext.Post.FirstOrDefault(p => p.Id == postId && p.UserId == userId);

            ArgumentNullException.ThrowIfNull(post);

            post.Content = model.Content;
            DateTime now = DateTime.Now;
            post.UpdatedAt = now;

            _phorumContext.Post.Update(post);
            _phorumContext.SaveChanges();
        }
    }
}
