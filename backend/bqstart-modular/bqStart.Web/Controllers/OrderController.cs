using BinaryQuest.Framework.ModularCore.Implementation;
using BinaryQuest.Framework.ModularCore.Interface;
using bqStart.Data;

namespace bqStart.Web.Controllers;
public class OrderController : GenericDataController<Order, int>
{
    public OrderController(IApplicationService applicationService, ILogger<OrderController> logger, MainDataContext context) :
    base(applicationService, logger, new UnitOfWork(context))
    {

    }

    protected override dynamic OnGetLookupData()
    {
        var info = CurrentUser;
        var ret = new
        {
        };
        return ret;
    }
}
