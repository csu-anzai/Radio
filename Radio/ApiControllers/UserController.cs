namespace Radio.ApiControllers
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using Radio.Models.User;
    using Radio.Services;

    [ApiController]
    [Route("[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserImageService _userImageService;

        public UserController(IUserImageService userImageService)
        {
            _userImageService = userImageService;
        }

        [HttpGet]
        public bool IsLoggedIn()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier) != null;
        }

        [HttpGet]
        public UserThumbnail Thumbnail()
        {
            Claim idClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim == null)
            {
                return new UserThumbnail("/user-images/anonymous.png", "Anonymous");
            }

            string id = idClaim.Value;
            string username = User.FindFirst(ClaimTypes.Name).Value;

            string userImageFilename = _userImageService.UserImageFilenameForUserId(id);

            return new UserThumbnail($"/user-images/{userImageFilename}", username);
        }
    }
}