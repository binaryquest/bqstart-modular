using BinaryQuest.Framework.ModularCore.Implementation;
using BinaryQuest.Framework.ModularCore.Interface;
using bqStart.Data;

namespace bqStart.Web.Controllers;

public class ExampleClassController : GenericDataController<ExampleClass, int>
{
    public ExampleClassController(IApplicationService applicationService, ILogger<ExampleClassController> logger, MainDataContext context) :
    base(applicationService, logger, new UnitOfWork(context))
    {
        ExpandedTables = "Department";
        ExpandedTablesForSingleEntity = "Department";
    }

    protected override dynamic OnGetLookupData()
    {
        var deps = this.unitOfWork.GenericRepository<Department>().Get();

        var ret = new
        {
            departmentList = from d in deps
                         select new
                         {
                             id = d.Id,
                             name = d.DepartmentName
                         }
        };
        return ret;
    }

    protected override void OnBeforeCreate(ExampleClass entity)
    {
        
    }
}
