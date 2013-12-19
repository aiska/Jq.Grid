using System;
namespace Jq.Grid
{
	public class TreeGridSettings
	{
		public bool Enabled { get; set; }
		public string CollapsedIcon { get; set; }
		public string ExpandedIcon { get; set; }
		public string LeafIcon { get; set; }
		public TreeGridSettings()
		{
			this.Enabled = false;
			this.CollapsedIcon = "";
			this.ExpandedIcon = "";
			this.LeafIcon = "";
		}
	}
}
