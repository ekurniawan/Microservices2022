using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;

        public EventProcessor(IMapper mapper)
        {
            _mapper = mapper;
        }
        public void ProccessEvent(string message)
        {
            //var eventType 
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Menentukan Event");
            //var eventType = JsonSerializer.Deserialize
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}