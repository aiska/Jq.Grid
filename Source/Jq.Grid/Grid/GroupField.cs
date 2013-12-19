using System;
namespace Jq.Grid
{
	public class GroupField
	{
		public string DataField { get; set; }
		public string HeaderText { get; set; }
		public bool ShowGroupColumn { get; set; }
		public SortDirection GroupSortDirection { get; set; }
		public bool ShowGroupSummary { get; set; }
		public GroupField()
		{
			this.DataField = "";
			this.HeaderText = "<b>{0}</b>";
			this.ShowGroupColumn = true;
			this.GroupSortDirection = SortDirection.Asc;
			this.ShowGroupSummary = false;
		}
	}
}
