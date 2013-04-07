namespace FluentValidationExperiment.Advanced.Requests
{
    public class SignUpRequest
    {
        public string Email { get; set; }
        public string Password1 { get; set; }
        public string Password2 { get; set; }
    }
}