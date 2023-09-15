using BinaryQuest.Framework.ModularCore.Data;
using BinaryQuest.Framework.ModularCore.Model;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Interface
{
    public interface IApplicationService: ICacheManager
    {
        Bootdata Bootdata { get; }
        AppConfigOptions ConfigOptions { get; }        
        Type DbContextType { get; }        
    }    
}
