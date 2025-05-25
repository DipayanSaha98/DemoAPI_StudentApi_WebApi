namespace SchoolManagement.Model                                   //In C# (and .NET in general), folder names do NOT define namespaces . so using "Model" explicitly instead of "Models" .
{
    public class LoginResponseDTO
    {
        public LocalUser UserDetails { get; set; }
        public string Token { get; set; }
    }
}
