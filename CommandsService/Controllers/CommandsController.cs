using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/platforms/{platformId}/[controller]")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;
        public CommandsController(ICommandRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Semua Command dari platform {platformId}");
            if(!_repository.PlatformExist(platformId))
                return NotFound();

            var commands = _repository.GetCommandsForPlatform(platformId);
            var commandReadDto = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return Ok(commandReadDto);
        }

        [HttpGet("{commandId}")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId,int commandId)
        {
             Console.WriteLine($"--> Satu Command dari platform {platformId} / {commandId}");
             if(!_repository.PlatformExist(platformId))
                return NotFound();

             var command = _repository.GetCommand(platformId,commandId);
             if(command==null) return NotFound();

             return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, 
            CommandCreateDto commandCreateDto)
        {
             Console.WriteLine($"--> Menambahkan Command untuk platform {platformId}");

             if(!_repository.PlatformExist(platformId)) 
                return NotFound();

            var command = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(platformId,command);
            _repository.SaveChange();

            var commandReadDto = _mapper.Map<CommandReadDto>(command);

            return CreatedAtAction(nameof(GetCommandForPlatform),
                new {platformId = platformId, commandId=commandReadDto.Id},
                    commandReadDto);
        }
    }
}