using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo repository,IMapper mapper,
        ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Get Platforms");
            var results = _repository.GetAllPlatforms();
            var platformReadDtos = _mapper.Map<IEnumerable<PlatformReadDto>>(results);
            return Ok(platformReadDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<PlatformReadDto> GetPlatformById(int id)
        {
            var results = _repository.GetPlatformById(id);
            if(results==null) return NotFound();

            var platformReadDto = _mapper.Map<PlatformReadDto>(results);
            return platformReadDto;
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var newPlatform = _mapper.Map<Platform>(platformCreateDto);
            _repository.CreatePlatform(newPlatform);
            _repository.SaveChanges();

            var platformReadDto = _mapper.Map<PlatformReadDto>(newPlatform);

            try
            {
                 await _commandDataClient.SendPlatformToCommand(platformReadDto);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Tidak dapat mengirimkan Sync Data: {ex.Message}");
            }

            return CreatedAtAction(nameof(GetPlatformById),new { Id=platformReadDto.Id },
                platformReadDto);
        }
    }
}