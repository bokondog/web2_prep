namespace Template.Mvc.Models
{
    public class TemplateUser
    {
        public int Id { get; set; }
        public string IdentityUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public ICollection<Reservation>? Reservations { get; set; }
    }
}
