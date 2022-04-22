using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessor(IMapper mapper,IServiceScopeFactory scopeFactory)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
        }
        public void ProccessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch(eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExist(platform.ExternalID))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChange();
                        Console.WriteLine("--> Menambahkan Platform Baru - CommandsService");
                    }
                    else
                    {
                        Console.WriteLine("--> Platform sudah ada di CommandsService");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Tidak dapat menambahkan platform ke database {ex.Message}");
                }
            }

            
            
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Menentukan Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> Platform Published Event detected...");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> Cant determined this event...");
                    return EventType.Undetermined;
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}