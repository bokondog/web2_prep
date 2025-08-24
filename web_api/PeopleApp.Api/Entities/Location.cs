namespace PeopleApp.Api.Entities
{
    public class Location
    {
        public long Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public IEnumerable<Person>? People { get; set; }
    }
}
