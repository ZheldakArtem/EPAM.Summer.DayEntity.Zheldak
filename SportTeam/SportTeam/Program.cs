using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportTeam.EDMX;

namespace SportTeam
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SportTeamEntities cx = new SportTeamEntities())
            {
                Team team = cx.Teams.FirstOrDefault();
                var entityType = ObjectContext.GetObjectType(team.GetType());

                //FirstOrDefault
                var l2EQuery = cx.Players.Where(s => s.TeamId == 1);
                var players = l2EQuery.FirstOrDefault();
                Console.WriteLine(players.Name);

                //SingleOrDefault
                var single = cx.Players.SingleOrDefault(s => s.Age == 23);
                Console.WriteLine(single.Name);

                //To List
                var list = cx.Teams.Where(c => c.Name == "FC BATE").ToList();
                foreach (var item in list)
                {
                    Console.WriteLine("The comand {0} ,have a coach {1}", item.Name, item.Coach);
                }

                //Group By
                var group = cx.Players.GroupBy(p => p.Position);
                Console.WriteLine("Count posotion {0}", group.Count());

                //OrderBy
                var order = cx.Players.OrderBy(o => o.Age);
                foreach (var item in order)
                {
                    Console.WriteLine("Age {0} => {1}", item.Age, item.Name);
                }

                //Anonymous Class
                var anonymous = cx.Players.Select(c => new { c.Name });
                foreach (var item in anonymous)
                {
                    Console.WriteLine(item.Name);
                }
                Console.WriteLine("<<<<<DBEntityEntry>>>>>");

                #region DBEntityEntry
                //get student whose PlayerId is 2
                var player = cx.Players.Find(3);

                //edit student name
                player.Name = "VIKA";

                //get DbEntityEntry object for student entity object
                var entry = cx.Entry(player);

                //get entity information e.g. full name
                Console.WriteLine("Entity Name: {0}", entry.Entity.GetType().FullName);

                //get current EntityState
                Console.WriteLine("Entity State: {0}", entry.State);

                Console.WriteLine("********Property Values********");

                foreach (var propertyName in entry.CurrentValues.PropertyNames)
                {
                    Console.WriteLine("Property Name: {0}", propertyName);

                    //get original value
                    var orgVal = entry.OriginalValues[propertyName];
                    Console.WriteLine("     Original Value: {0}", orgVal);

                    //get current values
                    var curVal = entry.CurrentValues[propertyName];
                    Console.WriteLine("     Current Value: {0}", curVal);
                }
                #endregion

                #region CRUD Operation in Connected Scenario
                var playerList = cx.Players.ToList();

                //Perform create operation
                cx.Players.Add(new Player() { Name = "New Player", Age = 44 });

                //Perform Update operation
                var playerToUpdate = playerList.FirstOrDefault(s => s.Name == "VIKA");
                playerToUpdate.Name = "Vova";

                //Perform delete operation
                cx.Players.Remove(playerList.ElementAt(4));

                //Execute Inser, Update & Delete queries in the database
                cx.SaveChanges();

                #endregion

                #region Lazy Loading
                IList<Player> studList = cx.Players.ToList<Player>();

                Player std = studList[0];

                Team add = std.Team;
                #endregion
            }
                #region Cascade deleting
            using (SportTeamEntities context = new SportTeamEntities())
            {

                var newPlater = new Player() { Name = "DDDD" };
                var newTeam = new Team() { Name = "DDDTEAM" };
                newPlater.Team = newTeam;
                context.Players.Add(newPlater);
                context.SaveChanges();
                context.Teams.Remove(newTeam);
                context.SaveChanges();

                var footPlayer = context.Players.Include(s => s.Team).FirstOrDefault(s => s.Age == 21);

                Console.WriteLine(footPlayer.Name);

                #endregion
            }
        }
    }
}
