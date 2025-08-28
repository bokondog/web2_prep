namespace Template.Mvc.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        public string UserName => $"{FirstName}_{LastName}";
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}