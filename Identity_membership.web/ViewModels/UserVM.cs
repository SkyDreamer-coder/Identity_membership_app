namespace Identity_membership.web.ViewModels
{
    public class UserVM
    {
        public UserVM() { }

        public UserVM(string userName, string email, string phoneNumber)
        {
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
        }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
