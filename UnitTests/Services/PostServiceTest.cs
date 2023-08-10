using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Phorum.Entities;
using Phorum.Helpers;
using Phorum.Models;
using Phorum.Repositories.PostRepository;
using Phorum.Repositories.UserRepository;
using Phorum.Services;

namespace UnitTests.Services
{
    [TestClass]
    public class PostServiceTest
    {
        private IPostService _postService;
        private Mock<IPostRepository> _postRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IHttpContextHelper> _httpContextHelperMock;

        [TestInitialize]
        public void TestSetup()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _httpContextHelperMock = new Mock<IHttpContextHelper>();
            var config = new MapperConfiguration(cfg =>
            {
              cfg.CreateMap<Post, PostDTO>()
             .ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDTO
             {
                 Id = src.User.Id,
                 Name = src.User.Name,
                 Email = src.User.Email,
             })).ReverseMap();
            });
            
            var mapper = config.CreateMapper();
            _postService = new PostService(mapper, _httpContextHelperMock.Object, _postRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [TestMethod]
        public void CreatePost_should_create_the_post_and_save_to_the_database()
        {
            // Arrange
            User user = new()
            {
                DateOfBirth = DateTime.Now,
                Email = "placeholder@email.com",
                Name = "Jhon doe",
                Password = "123",
                Id = 1
                
            };

            _userRepositoryMock.Setup(repo => repo.GetUser(It.Is<int>(value=> value==user.Id))).Returns(user);
            _postRepositoryMock.Setup(repo => repo.CreatePost(It.IsAny<Post>()));
            _postRepositoryMock.Setup(repo => repo.SaveChanges());
            _httpContextHelperMock.Setup(helper => helper.GetUserId()).Returns(user.Id);
            // Act
            _postService.CreatePost("new Post");
            // Assert
            _postRepositoryMock.Verify(repo => repo.CreatePost(It.IsAny<Post>()), Times.Once());
            _postRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
            _userRepositoryMock.Verify(repo => repo.GetUser(user.Id));
        }
        [TestMethod]
        public void UpdatePost_should_create_the_post_and_save_to_the_database()
        {
            // Arrange
            User user = new()
            {
                DateOfBirth = DateTime.Now,
                Email = "placeholder@email.com",
                Name = "Jhon doe",
                Password = "123",
                Id = 1

            };
            Post post = new()
            {
                User = user,
                Content = "content",
                Id = 1,
                UserId = user.Id
            };

            _userRepositoryMock.Setup(repo => repo.GetUser(It.Is<int>(value => value == user.Id))).Returns(user);
            _postRepositoryMock.Setup(repo => repo.GetPostById(It.Is<int>(value => value== post.Id))).Returns(post);
            _postRepositoryMock.Setup(repo => repo.SaveChanges());
            _httpContextHelperMock.Setup(helper => helper.GetUserId()).Returns(user.Id);
            // Act
            _postService.UpdatePost("new Post", post.Id);
            // Assert
            _postRepositoryMock.Verify(repo => repo.UpdatePost(It.IsAny<Post>()), Times.Once());
            _postRepositoryMock.Verify(repo => repo.SaveChanges(), Times.Once());
        }

        [TestMethod]
        public void GetPostById_ValidId_ReturnsPostDTO()
        {
            // Arrange
            int postId = 1;
            Post samplePost = new() { Id = postId, Content = "Test Post" };
            _postRepositoryMock.Setup(repo => repo.GetPostById(postId)).Returns(samplePost);

            // Act
            PostDTO result = _postService.GetPostById(postId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(samplePost.Content, result.Content);
        }

        [TestMethod]
        public void GetAllPosts_ReturnsPostDTOList()
        {
            // Arrange
            User user = new()
            {
                DateOfBirth = DateTime.Now,
                Email = "placeholder@email.com",
                Name = "Jhon doe",
                Password = "123",
                Id = 1

            };
            Post samplePost1 = new() { Id = 1, Content = "Test Post", User = user };
            Post samplePost2 = new() { Id = 2, Content = "Test Post", User = user };
            List<Post> posts = new() { samplePost1, samplePost2 };
            _postRepositoryMock.Setup(repo => repo.GetAllPosts()).Returns(posts);

            // Act
            ICollection<PostDTO> result = _postService.GetAllPosts();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(posts.Count, result.Count);
        }
    }
}