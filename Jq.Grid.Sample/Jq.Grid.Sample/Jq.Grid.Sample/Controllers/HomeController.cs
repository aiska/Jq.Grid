using Jq.Grid.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jq.Grid.Sample.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            JQGridModel<invheader> model = new JQGridModel<invheader>("JsonExample");
            model.Grid.DataUrl = Url.Action("Data");
            return View(model);
        }

        [HttpPost]
        public JsonResult Data()
        {
            JQGridModel<invheader> model = new JQGridModel<invheader>();
            using (DataContext ctx = new DataContext())
            {
                return model.Grid.DataBind(ctx.InvoiceHeader.AsQueryable());
            }
        }
    }
}
