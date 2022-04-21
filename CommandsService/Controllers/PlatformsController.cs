using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [ApiController]
    [Route("api/c/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Ambil platforms dari Command Service");
            var results = _repository.GetAllPlatforms();
            var platformReadDto = _mapper.Map<IEnumerable<PlatformReadDto>>(results);
            return Ok(platformReadDto);
        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST CommandService");
            return Ok("Inbound test dari Platform Controller");        
        }

        
    }
}