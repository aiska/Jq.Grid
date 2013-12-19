using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
namespace Jq.Grid
{
	public class JQAutoComplete
	{
		public AutoCompleteMode AutoCompleteMode { get; set; }
		public string DataField { get; set; }
		public object DataSource { get; set; }
		public string DataUrl { get; set; }
		public int Delay { get; set; }
		public AutoCompleteDisplayMode DisplayMode { get; set; }
		public bool Enabled { get; set; }
		public string ID { get; set; }
		public int MinLength { get; set; }
		public JQAutoComplete()
		{
			this.AutoCompleteMode = AutoCompleteMode.BeginsWith;
			this.DataField = "";
			this.DataSource = null;
			this.DataUrl = "";
			this.Delay = 300;
			this.DisplayMode = AutoCompleteDisplayMode.Standalone;
			this.Enabled = true;
			this.ID = "";
			this.MinLength = 1;
		}
		public JsonResult DataBind(object dataSource)
		{
			this.DataSource = dataSource;
			return this.DataBind();
		}
		public JsonResult DataBind()
		{
			return this.GetJsonResponse();
		}

        [HttpPost]
		private JsonResult GetJsonResponse()
		{
			Guard.IsNotNull(this.DataSource, "DataSource");
			IQueryable queryable = this.DataSource as IQueryable;
			Guard.IsNotNull(queryable, "DataSource", "should implement the IQueryable interface.");
			Guard.IsNotNullOrEmpty(this.DataField, "DataField", "should be set to the datafield (column) of the datasource to search in.");
			SearchOperation searchOperation;
			if (this.AutoCompleteMode == AutoCompleteMode.BeginsWith)
			{
				searchOperation = SearchOperation.BeginsWith;
			}
			else
			{
				searchOperation = SearchOperation.Contains;
			}
			string text = HttpContext.Current.Request.QueryString["term"];
			if (!string.IsNullOrEmpty(text))
			{
				queryable = queryable.Where(Util.ConstructLinqFilterExpression(this, new Util.SearchArguments
				{
					SearchColumn = this.DataField,
					SearchOperation = searchOperation,
					SearchString = text
				}), new object[0]);
			}
			JsonResult jsonResult = new JsonResult();
			jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
			jsonResult.Data = queryable.ToListOfString(this);
			return jsonResult;
		}
	}
}
