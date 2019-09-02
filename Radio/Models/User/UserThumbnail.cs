namespace Radio.Models.User
{
    public class UserThumbnail
    {
        public UserThumbnail(AppUser user) : this($"/user-images/{user.Id}.png", user.UserName)
        {
        }

        public UserThumbnail(string imageUrl, string username)
        {
            ImageUrl = imageUrl;
            Username = username;
        }

        public string ImageUrl { get; }

        public string Username { get; }
    }
}