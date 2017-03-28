using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using SwaggerDemo.Models;

namespace SwaggerDemo.Controllers
{
    /// <summary>
    /// Add, update, and remove information about players
    /// </summary>
    public class PlayersController : DemoController
    {
        private static readonly IEnumerable<Player> SamplePlayers = new []
        {
            new Player()
            {
                Id = 1,
                TeamId = 1,
                FirstName = "Troy",
                LastName = "Polamalu",
                Joined = new DateTime(2003, 4, 26),
                IsCaptain = false,
                Position = "Safety",
                Dob = new DateTime(1981, 4, 19),
            },
            new Player()
            {
                Id = 2,
                TeamId = 1,
                FirstName = "Hines",
                LastName = "Ward",
                Joined = new DateTime(1998, 4, 18),
                IsCaptain = true,
                Position = "Wide Receiver",
                Dob = new DateTime(1976, 3, 8),
            },
            new Player()
            {
                Id = 3,
                TeamId = 3,
                FirstName = "Mike",
                LastName = "Piazza",
                Joined = new DateTime(1992, 9, 1),
                IsCaptain = true,
                Position = "Catcher",
                Dob = new DateTime(1968, 9, 4),
                Email = "mikep@mets.com",
                PhoneNumbers = new [] { "(111) 111-1111" },
            },
            new Player()
            {
                Id = 4,
                TeamId = 3,
                FirstName = "Keith",
                LastName = "Hernandez",
                Joined = new DateTime(1989, 8, 30),
                IsCaptain = false,
                Position = "First Baseman",
                Dob = new DateTime(1953, 10, 20),
                Email = "keithh@mets.com",
                PhoneNumbers = new [] { "(234) 555-6666" },
                Address = new Address() { Street = "123 Main St.", City = "New York", Region = "NY", Postal = "11111", Country = "USA" }
            },
        };

        private static int NextId() => SamplePlayers.Max(t => t.Id).GetValueOrDefault() + 1;

        /// <summary>
        /// Get a list of all the players, or players on a team
        /// </summary>
        /// <param name="teamId">Optional team ID to search by</param>
        /// <param name="firstName">Optional first name to search by</param>
        /// <param name="lastName">Optional last name to search by</param>
        /// <param name="position">Optional position to search by</param>
        /// <returns>Zero or more players</returns>
        [HttpGet, Route("players")]
        [ResponseType(typeof(IEnumerable<Player>))]
        public IHttpActionResult Get(int? teamId = null, string firstName = null, string lastName = null, string position = null)
        {
            Func<Player, bool> WhereClause = p =>
            {
                var b = true;
                if (teamId.HasValue) b &= p.TeamId == teamId.Value;
                if (!string.IsNullOrEmpty(firstName)) b &= p.FirstName == firstName;
                if (!string.IsNullOrEmpty(lastName)) b &= p.LastName == lastName;
                if (!string.IsNullOrEmpty(position)) b &= p.Position == position;
                return b;
            };

            var playersOnTeam = SamplePlayers.Where(WhereClause).ToList();
            return playersOnTeam.Any() ?
                Ok(playersOnTeam) :
                NotFound($"Could not find any players with provided criteria");
        }

        /// <summary>
        /// Looks up a player by ID
        /// </summary>
        /// <param name="id">Player ID to look up</param>
        /// <returns>A player</returns>
        [HttpGet, Route("players/{id}", Name = "GetPlayer")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult Get(int id)
        {
            var player = SamplePlayers.FirstOrDefault(l => l.Id == id);
            return player == default(Player) ?
                NotFound($"Could not find player {id}") :
                Ok(player);
        }

        /// <summary>
        /// Adds a player to the service
        /// </summary>
        /// <param name="player">The player to add to the service</param>
        /// <returns>The player that was added</returns>
        [HttpPut, Route("players")]
        [ResponseType(typeof(Player))]
        public IHttpActionResult Put(Player player)
        {
            if (player == null)
                return BadRequest("Player is required");

            if (SamplePlayers.Any(l => l.Id == player.Id))
                return BadRequest("Player ID already in use");

            if (!player.Id.HasValue)
                player.Id = NextId();

            return Created(Url.Route("GetPlayer", new { id = player.Id }), player);
        }

        /// <summary>
        /// Removes a player from the service
        /// </summary>
        /// <param name="id">The player id to remove from the service</param>
        [HttpDelete, Route("players/{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (id < 0)
                return BadRequest("Invalid id");

            return Ok();
        }
    }
}
