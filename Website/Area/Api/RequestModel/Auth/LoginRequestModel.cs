using Anil.Web.Framework.Models;

namespace Website.Area.Api.RequestModel.Auth
{
    public record class LoginRequestModel: BaseAnilModel
    {
        public LoginRequestModel(): base()
        {

        }

        public LoginRequestModel(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
