using ImageService.Infrastructure.CommandsInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetConfigCommand : ICommand
    {
        public string Execute(string[] args, out bool result)
        {
            result = true;
            //send app config
            ConfigData configData = ConfigData.InstanceConfig;
            return configData.ToJSON();

        }

        
    }
}
