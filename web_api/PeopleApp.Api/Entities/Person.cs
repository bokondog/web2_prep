namespace PeopleApp.Api.Entities
{
    public class Person
    {
        public long Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public Department Department { get; set; }
        public long DepartmentId { get; set; }
        public Location Location { get; set; }
        public long LocationId { get; set; }
    }
}
