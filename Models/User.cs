namespace MyChat.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public User(string id,string password,string email)
        {
            Id = id;
            Password = password;
            Email = email;
        }

        public User()
        {
           
        }
    }
}
