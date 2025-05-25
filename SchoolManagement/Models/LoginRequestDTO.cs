namespace SchoolManagement.Model                    //In C# (and .NET in general), folder names do NOT define namespaces . so using "Model" explicitly instead of "Models" .
{
    public class LoginRequestDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
