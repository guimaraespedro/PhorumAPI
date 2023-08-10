using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Phorum.Entities;
using Phorum.Helpers;
using Phorum.Models;
using Phorum.Repositories.PostRepository;
using Phorum.Repositories.UserRepository;
using System.Security.Claims;

namespace Phorum.Services
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IHttpContextHelper _httpContextHelper;
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        public PostService(IMapper mapper, IHttpContextHelper httpContextHelper, IPostRepository postRepository, IUserRepository userRepository) {
            _mapper = mapper;
            _httpContextHelper = httpContextHelper;
            _postRepository = postRepository;
            _userRepository = userRepository;
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
            User? user = _userRepository.GetUser(userId);

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


        public void UpdatePost(string newContent, int postId)
        {
            int userId = _httpContextHelper.GetUserId();

            Post? post = _postRepository.GetPostById(postId);
            ArgumentNullException.ThrowIfNull(post);

            if(post.User.Id != userId)
            {
                throw new Exception("cannot update a post that is not yours");
            }


            post.Content = newContent;
            DateTime now = DateTime.Now;
            post.UpdatedAt = now;

            _postRepository.UpdatePost(post);
            _postRepository.SaveChanges();
        }
    }
}
