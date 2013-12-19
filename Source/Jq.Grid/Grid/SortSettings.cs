using System;
namespace Jq.Grid
{
	public class SortSettings
	{
		public bool AutoSortByPrimaryKey { get; set; }
		public string InitialSortColumn { get; set; }
		public SortDirection InitialSortDirection { get; set; }
		public SortIconsPosition SortIconsPosition { get; set; }
		public SortAction SortAction { get; set; }
		public SortSettings()
		{
			this.AutoSortByPrimaryKey = true;
			this.InitialSortColumn = "";
			this.InitialSortDirection = SortDirection.Asc;
			this.SortIconsPosition = SortIconsPosition.Vertical;
			this.SortAction = SortAction.ClickOnHeader;
		}
	}
}
