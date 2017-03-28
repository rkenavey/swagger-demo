using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using SwaggerDemo.Models;

namespace SwaggerDemo.Controllers
{
    /// <summary>
    /// Add, update, and remove information about teams
    /// </summary>
    public class TeamsController : DemoController
    {
        private static readonly IEnumerable<Team> SampleTeams = new []
        {
            new Team()
            {
                Id = 1,
                LeagueId = 1,
                Name = "Pittsburgh Steelers",
                Mascot = "Steely McBeam",
                Address = new Address() { Street = "100 Art Rooney Avenue", City = "Pittsburgh", Region = "PA", Postal = "15212", Country = "USA" }
            },
            new Team()
            {
                Id = 2,
                LeagueId = 1,
                Name = "Denver Broncos",
                Mascot = "Miles",
                Address = new Address() { Street = "13655 Broncos Parkway", City = "Englewood", Region = "CO", Postal = "80112", Country = "USA" }
            },
            new Team()
            {
                Id = 3,
                LeagueId = 2,
                Name = "NY Mets",
                Mascot = "Mr. Met",
                Address = new Address() { Street = "Roosevelt Avenue", City = "Flushing", Region = "NY", Postal = "11368", Country = "USA" }
            },
            new Team()
            {
                Id = 4,
                LeagueId = 2,
                Name = "New Jersey Devils",
                Mascot = "NJ Devil",
                Address = new Address() { Street = "25 Lafayette St", City = "Newark", Region = "NJ", Postal = "07102", Country = "USA" }
            },
        };

        private static int NextId() => SampleTeams.Max(t => t.Id).GetValueOrDefault() + 1;

        /// <summary>
        /// Get a list of all the teams, or teams within a league
        /// </summary>
        /// <param name="leagueId">Optional league ID to search by</param>
        /// <returns>Zero or more teams</returns>
        [HttpGet, Route("teams")]
        [ResponseType(typeof(IEnumerable<Team>))]
        public IHttpActionResult Get(int? leagueId = null)
        {
            if (!leagueId.HasValue)
                return Ok(SampleTeams); // All teams

            var teamsInLeague = SampleTeams.Where(t => t.LeagueId == leagueId).ToList();
            return teamsInLeague.Any() ?
                Ok(teamsInLeague) :
                NotFound($"Could not find any teams in league {leagueId.Value}");
        }

        /// <summary>
        /// Looks up a team by ID
        /// </summary>
        /// <param name="id">Team ID to look up</param>
        /// <returns>A team</returns>
        [HttpGet, Route("teams/{id}", Name = "GetTeam")]
        [ResponseType(typeof(Team))]
        public IHttpActionResult Get(int id)
        {
            var team = SampleTeams.FirstOrDefault(l => l.Id == id);
            return team == default(Team) ?
                NotFound($"Could not find team {id}") :
                Ok(team);
        }

        /// <summary>
        /// Adds a team to the service
        /// </summary>
        /// <param name="team">The team to add to the service</param>
        /// <returns>The team that was added</returns>
        [HttpPut, Route("teams")]
        [ResponseType(typeof(Team))]
        public IHttpActionResult Put(Team team)
        {
            if (team == null)
                return BadRequest("Team is required");

            if (SampleTeams.Any(l => l.Id == team.Id))
                return BadRequest("Team ID already in use");

            if (!team.Id.HasValue)
                team.Id = NextId();

            return Created(Url.Route("GetTeam", new { id = team.Id }), team);
        }

        /// <summary>
        /// Removes a team from the service
        /// </summary>
        /// <param name="id">The team id to remove from the service</param>
        [HttpDelete, Route("teams/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Invalid Team ID");

            return Ok();
        }
    }
}
