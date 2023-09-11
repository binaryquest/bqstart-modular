using BinaryQuest.Framework.ModularCore.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bqStart.Data
{
    public sealed class ApplicationDbInitializer
    {
        public static async Task Initialize(IApplicationService applicationService, MainDataContext context,  ILogger<ApplicationDbInitializer> dbInitializerLogger, IConfiguration configuration)
        {
            await context.Database.MigrateAsync();
        }
    }
}
