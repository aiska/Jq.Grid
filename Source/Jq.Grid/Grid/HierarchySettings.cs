using System;
using System.Collections;
using Newtonsoft.Json;
namespace Jq.Grid
{
	public class HierarchySettings
	{
		public HierarchyMode HierarchyMode { get; set; }
		public string PlusIcon { get; set; }
		public string MinusIcon { get; set; }
		public string OpenIcon { get; set; }
		public bool ExpandOnLoad { get; set; }
		public bool SelectOnExpand { get; set; }
		public bool ReloadOnExpand { get; set; }
		public HierarchySettings()
		{
			this.HierarchyMode = HierarchyMode.None;
			this.PlusIcon = "ui-icon-plus";
			this.MinusIcon = "ui-icon-minus";
			this.OpenIcon = "ui-icon-carat-1-sw";
			this.ExpandOnLoad = false;
			this.SelectOnExpand = false;
			this.ReloadOnExpand = true;
		}
		internal string ToJSON()
		{
			return JsonConvert.SerializeObject(this.ToHashtable());
		}
		internal Hashtable ToHashtable()
		{
			Hashtable hashtable = new Hashtable();
			if (this.PlusIcon != null && this.PlusIcon != "ui-icon-plus")
			{
				hashtable.Add("plusicon", this.PlusIcon);
			}
			if (this.MinusIcon != null && this.MinusIcon != "ui-icon-minus")
			{
				hashtable.Add("minusicon", this.MinusIcon);
			}
			if (this.OpenIcon != null && this.OpenIcon != "ui-icon-carat-1-sw")
			{
				hashtable.Add("openicon", this.OpenIcon);
			}
			if (this.ExpandOnLoad)
			{
				hashtable.Add("expandOnLoad", true);
			}
			if (this.SelectOnExpand)
			{
				hashtable.Add("selectOnExpand", true);
			}
			if (!this.ReloadOnExpand)
			{
				hashtable.Add("reloadOnExpand", false);
			}
			return hashtable;
		}
	}
}
