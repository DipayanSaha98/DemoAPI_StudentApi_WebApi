namespace DemoAPI.Model
{
    public class UserLoginResponse
    {
        public LocalUser UserDetails { get; set; }
        public string Token { get; set; }
    }
}
