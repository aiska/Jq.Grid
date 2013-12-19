using System;
namespace Jq.Grid
{
	internal class JsonToolBar
	{
		public bool edit { get; set; }
		public bool add { get; set; }
		public bool del { get; set; }
		public bool search { get; set; }
		public bool refresh { get; set; }
		public bool view { get; set; }
		public string position { get; set; }
		public bool cloneToTop { get; set; }
		public JsonToolBar(ToolBarSettings settings)
		{
			this.edit = settings.ShowEditButton;
			this.add = settings.ShowAddButton;
			this.del = settings.ShowDeleteButton;
			this.search = settings.ShowSearchButton;
			this.refresh = settings.ShowRefreshButton;
			this.view = settings.ShowViewRowDetailsButton;
			this.position = settings.ToolBarAlign.ToString().ToLower();
			this.cloneToTop = true;
		}
	}
}
