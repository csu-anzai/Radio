namespace Radio.Models.User
{
    public class UserThumbnail
    {
        public UserThumbnail(string imageUrl, string username)
        {
            ImageUrl = imageUrl;
            Username = username;
        }

        public string ImageUrl { get; }

        public string Username { get; }
    }
}