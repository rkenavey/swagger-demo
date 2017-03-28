using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using SwaggerDemo.Models;

namespace SwaggerDemo.Controllers
{
    /// <summary>
    /// Add, update, and remove information about leagues
    /// </summary>
    public class LeaguesController : DemoController
    {
        private static readonly IEnumerable<League> SampleLeagues = new []
        {
            new League()
            {
                Id = 1,
                Name = "NFL",
                Sport = Sport.Football,
            },
            new League()
            {
                Id = 2,
                Name = "MLB",
                Sport = Sport.Baseball,
            },
            new League()
            {
                Id = 3,
                Name = "NBA",
                Sport = Sport.Basketball,
            },
            new League()
            {
                Id = 4,
                Name = "NHL",
                Sport = Sport.Hockey,
            },
        };

        private static int NextId() => SampleLeagues.Max(t => t.Id).GetValueOrDefault() + 1;

        /// <summary>
        /// Get a list of all the leagues
        /// </summary>
        /// <returns>Zero or more leagues</returns>
        [HttpGet, Route("leagues")]
        [ResponseType(typeof(IEnumerable<League>))]
        public IHttpActionResult Get()
        {
            return Ok(SampleLeagues);
        }

        /// <summary>
        /// Looks up a league by ID
        /// </summary>
        /// <param name="id">League ID to look up</param>
        /// <returns>A league</returns>
        [HttpGet, Route("leagues/{id}", Name = "GetLeague")]
        [ResponseType(typeof(League))]
        public IHttpActionResult Get(int id)
        {
            var league = SampleLeagues.FirstOrDefault(l => l.Id == id);

            return league == default(League) ?
                NotFound($"Could not find league {id}") :
                Ok(league);
        }

        /// <summary>
        /// Adds a league to the service
        /// </summary>
        /// <param name="league">The league to add to the service</param>
        /// <returns>The league that was added</returns>
        [HttpPut, Route("leagues")]
        [ResponseType(typeof(League))]
        public IHttpActionResult Put(League league)
        {
            if (league == null)
                return BadRequest("League is required");

            if (SampleLeagues.Any(l => l.Id == league.Id))
                return BadRequest("League ID already in use");

            if (!league.Id.HasValue)
                league.Id = NextId();

            return Created(Url.Route("GetLeague", new { id = league.Id }), league);
        }

        /// <summary>
        /// Removes a league from the service
        /// </summary>
        /// <param name="id">The league id to remove from the service</param>
        [HttpDelete, Route("leagues/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Invalid League ID");

            return Ok();
        }
    }
}
