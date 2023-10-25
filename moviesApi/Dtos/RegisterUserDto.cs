namespace moviesApi.Dtos
{
    public class RegisterUserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
    }
}
