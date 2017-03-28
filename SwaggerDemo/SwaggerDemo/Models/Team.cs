namespace SwaggerDemo.Models
{
    public class Team
    {
        public int? Id { get; set; }
        public int LeagueId { get; set; }
        public string Name { get; set; }
        public string Mascot { get; set; }
        public Address Address { get; set; }
    }
}
