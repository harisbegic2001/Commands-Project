using System.Collections.Generic;
using Commander.Models;

namespace Commander.Data
{ //Zašto nema annotaciju 
    public interface ICommanderRepo{ //Pogledati demo repository --> da li je slično strukturi services i da li se mogu koristiti termini services i repositories interchangeably 
        IEnumerable<Command> GetAllCommands();
        Command GetCommandbyId(int id);
        void CreateCommand(Command cmd);
        bool SaveChanges();
        void UpdateCommand(Command cmd);

        void DeleteCommand(Command cmd);
    }
}