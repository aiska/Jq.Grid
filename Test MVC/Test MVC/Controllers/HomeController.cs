using Jq.Grid.Demo.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Jq.Grid.Demo.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            JsonExampleGridModel model = new JsonExampleGridModel();
            model.Grid.DataUrl = Url.Action("Data");
            return View(model);
        }

        [HttpPost]
        public JsonResult Data()
        {
            JsonExampleGridModel model = new JsonExampleGridModel();
            using (DataContext ctx = new DataContext())
            {
                return model.Grid.DataBind(ctx.InvoiceHeader.AsQueryable());
            }
        }
    }
}
