using BinaryQuest.Framework.ModularCore.Data;
using BinaryQuest.Framework.ModularCore.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryQuest.Framework.ModularCore.Interface
{
    public interface IApplicationService
    {
        Bootdata Bootdata { get; }
        AppConfigOptions ConfigOptions { get; }        
        Type DbContextType { get; }
    }    
}
