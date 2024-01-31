namespace Identity_membership.web.ViewModels
{
    public class UserVM
    {
        public UserVM() { }

        public UserVM(string userName, string email, string phoneNumber, string pictureUrl)
        {
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            PictureUrl = pictureUrl;
        }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PictureUrl { get; set; }
    }
}
