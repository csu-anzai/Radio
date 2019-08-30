namespace Radio.ApiControllers
{
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Mvc;

    using Radio.Models.User;
    using Radio.Services.FileProviders;

    [ApiController]
    [Route("[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IWebRootFileProvider _webRootFileProvider;

        public UserController(IWebRootFileProvider webRootFileProvider)
        {
            _webRootFileProvider = webRootFileProvider;
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

            string userImage = _webRootFileProvider.GetDirectoryContents("user-images")
                                                   .Select(entry => entry.Name)
                                                   .First(entryName => entryName.StartsWith(string.Concat(id, ".")));

            return new UserThumbnail($"/user-images/{userImage}", username);
        }
    }
}