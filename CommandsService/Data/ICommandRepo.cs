using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        bool SaveChange();
        
        //platform
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExist(int platformId);
        bool ExternalPlatformExist(int externalPlatformId);
        
        //command
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId,int commnadId);
        void CreateCommand(int platformId,Command command);
     }
}