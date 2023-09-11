using BinaryQuest.Framework.ModularCore.Implementation;
using BinaryQuest.Framework.ModularCore.Interface;
using bqStart.Data;
using Microsoft.AspNetCore.Authorization;

namespace bqStart.Web.Controllers;

public class DepartmentController : GenericDataController<Department, int>
{
    public DepartmentController(IApplicationService applicationService, ILogger<DepartmentController> logger, MainDataContext context) :
    base(applicationService, logger, new UnitOfWork(context))
    {

    }

    protected override dynamic OnGetLookupData()
    {
        var ret = new
        {                
        };
        return ret;
    }
}
