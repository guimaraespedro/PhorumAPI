using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phorum.Entities;
using Phorum.Helpers;
using Phorum.Models;
using System.Security.Claims;

namespace Phorum.Services
{
    public class PostService : IPostService
    {
        private readonly PhorumContext _phorumContext;
        private readonly IMapper _mapper;
        private readonly HttpContextHelper _httpContextHelper;
        public PostService(PhorumContext context, IMapper mapper, HttpContextHelper httpContextHelper) {
            _phorumContext = context;
            _mapper = mapper;
            _httpContextHelper = httpContextHelper;
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
            if (user == null)
            {
                throw new Exception();
            }
            post.User = user;

            _phorumContext.Post.Add(post);
            _phorumContext.SaveChanges();
        }

        public ICollection<PostDTO> GetAllPosts()
        {
            ICollection<PostDTO> posts = _phorumContext.Post.Include(p => p.User)
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
               }).ToList();

            return posts;
        }

        public PostDTO GetPostById(int postId)
        {
            PostDTO? post = _phorumContext.Post.Include(p => p.User)
               .Where(post => post.Id == postId)
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
               }).FirstOrDefault(); ;

            ArgumentNullException.ThrowIfNull(post);

            return post;
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
