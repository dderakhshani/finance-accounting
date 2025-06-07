namespace Eefa.Ticketing.Application.Contract.Dtos.BasicInfos
{
    public class LoginInput
    {
        public LoginInput(string username, string password)
        {
            Username = username;
            Password = password;
        }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class LoginResult
    {
        public bool succeed { get; set; }
        public string exceptions { get; set; }
        public string objResult { get; set; }
    }
}
