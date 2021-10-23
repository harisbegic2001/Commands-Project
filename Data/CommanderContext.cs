using Commander.Models;
using Microsoft.EntityFrameworkCore;

namespace Commander.Data{

    public class CommanderContext : DbContext{
        //In clip he said that DbContext is base class, Is ControllerBase in our Controller base class as well, What is Base class?
        public CommanderContext(DbContextOptions<CommanderContext> opt) : base (opt) 
        {
            
        }
        public DbSet<Command> Commands { get; set; } //We want to represent our command objects into our database as a DBSet and it's going to be called commands.
        
        
    }
}