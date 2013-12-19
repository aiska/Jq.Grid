using System;
using System.Web.Mvc;
namespace Jq.Grid
{
	public static class HtmlHelperExtensions
	{
		public static JqueryGridNamespace Grid(this HtmlHelper helper)
		{
			return new JqueryGridNamespace();
		}
	}
}
