using System.Security.Claims;

namespace Phorum.Helpers
{
    public class HttpContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAcessor;
        public HttpContextHelper(IHttpContextAccessor httpContextAccessor) {
            _httpContextAcessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            Claim? userId = _httpContextAcessor.HttpContext?.User.FindFirst("UserId");
            if (userId == null)
            {
                throw new Exception();
            }
            int parsedUserId = int.Parse(userId.Value);
            return parsedUserId;
        }
    }
}
