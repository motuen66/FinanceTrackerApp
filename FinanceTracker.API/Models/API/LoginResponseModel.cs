namespace FinanceTracker.API.Models.API
{
    public class LoginResponseModel
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public int ExpiresIn { get; set; }

        public LoginResponseModel() { }

        public LoginResponseModel(string Email, string Token, int ExpiresIn)
        {
            this.Email = Email;
            this.Token = Token;
            this.ExpiresIn = ExpiresIn;
        }
    }
}
