namespace inventory_aplication.Infrastructure.Services.CurrentUser
{
    using inventory_aplication.Application.Common.Interfaces.CurrentUser;
    using System.Security.Claims;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst("id");

                return userIdClaim is null
                    ? throw new UnauthorizedAccessException("User not authenticated")
                    : int.Parse(userIdClaim.Value);
            }
        }
    }

}
