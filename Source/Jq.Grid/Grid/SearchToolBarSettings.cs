using System;
namespace Jq.Grid
{
	public class SearchToolBarSettings
	{
		public SearchToolBarAction SearchToolBarAction { get; set; }
		public SearchToolBarSettings()
		{
			this.SearchToolBarAction = SearchToolBarAction.SearchOnEnter;
		}
	}
}
