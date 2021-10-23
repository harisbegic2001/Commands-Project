using System.Collections.Generic;
using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;


namespace Commander.Controllers{

// route in url:
    [Route("api/commands")]
    [ApiController] //@Controller java?
    public class CommandsController : ControllerBase{ //Šta je ControllerBase
    


//Nije neophodna keyword final?
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        //Ovdje se događa dependency Injection; Ne ide annotacija @Autowired 
        public CommandsController(ICommanderRepo repository, IMapper mapper) //trebalo bi mi ispisivati error jer ga ne usam
    {
        _repository = repository;
        _mapper = mapper;
        }
    
    //private final??
    //private readonly MockCommanderRepo _repository = new MockCommanderRepo(); /* zašto nije moglo 
    //private final(readonly) MockCommanderRepo mockCommanderRepo; */ 


    //Get api/commands
    [HttpGet] //RequestMapping  //Implementacija naših methods iz Services 
    public ActionResult <IEnumerable<CommandReadDto>> GetAllCommands(){
    var commandItems = _repository.GetAllCommands();
    return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }

    //Get api/commands/{id}
    [HttpGet("{id}", Name="GetCommandById")] //Da li je ovo jedanko requestmappingu u javi?? //Također implementacija naše method iz Services
    // Ili je zapravo route --> RequestMapping
    public ActionResult <CommandReadDto> GetCommandById(int id){
        var commandItem = _repository.GetCommandbyId(id);
        if(commandItem != null){
        return Ok(_mapper.Map<CommandReadDto>(commandItem));
        } 
        else return NotFound();
    }

    //POST api/commands
    [HttpPost]          /*Returnat će tip CommandReadDto*/ /*Očekujemo da user ubaci tip CommandCreateDto */
    public ActionResult <CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto)
    {      //We are mapping to Command Model and our source is commandCreateDto
        var commandModel = _mapper.Map<Command>(commandCreateDto); //Our command generates ID auto.
        _repository.CreateCommand(commandModel); //We create our Command in repo of type Command
        _repository.SaveChanges(); //We must saveChanges to our repo if we want our data to be saved in db
        

        //We don't want to return our commandCreateDto because it contains platform attribute
        var commandReadDto = _mapper.Map<CommandReadDto>(commandModel); //We don't want platform attribute
        return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id}, commandReadDto);
        
        //return Ok(commandReadDto);
    }
    //PUT api/commands/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto){
        var commandModelFromRepo = _repository.GetCommandbyId(id);
        if (commandModelFromRepo == null) { return NotFound();}
            
    _mapper.Map(commandUpdateDto, commandModelFromRepo);

    _repository.UpdateCommand(commandModelFromRepo);

    _repository.SaveChanges();

    return NoContent();
    }
    
    //PATCH api/commands/{id}
    [HttpPatch("{id}")]
    public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc){
    var commandModelFromRepo = _repository.GetCommandbyId(id);
    if (commandModelFromRepo == null) { return NotFound(); }  
    var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
    patchDoc.ApplyTo(commandToPatch, ModelState);
    if (!TryValidateModel(commandToPatch) ){
        return ValidationProblem(ModelState);
    }
    _mapper.Map(commandToPatch, commandModelFromRepo);
    _repository.UpdateCommand(commandModelFromRepo);
    _repository.SaveChanges();
    return NoContent();
    }

    
    //DELETE api/commands/{id}
    [HttpDelete("{id}")]
    public ActionResult DeleteCommand(int id){
    var commandModelFromRepo = _repository.GetCommandbyId(id);
    if (commandModelFromRepo == null) { return NotFound(); }
    _repository.DeleteCommand(commandModelFromRepo);
    _repository.SaveChanges();
    return NoContent();
        }
}   
}