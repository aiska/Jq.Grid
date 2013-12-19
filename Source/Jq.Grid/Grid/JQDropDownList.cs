using System;
using System.Collections;
using System.Collections.Generic;
namespace Jq.Grid
{
	public class JQDropDownList
	{
		public string ID { get; set; }
		public List<JQListItem> Items { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public int? DropDownWidth { get; set; }
		public int TabIndex { get; set; }
		public string ItemTemplateID { get; set; }
		public string HeaderTemplateID { get; set; }
		public string FooterTemplateID { get; set; }
		public string ToggleImageCssClass { get; set; }
		public DropDownListClientSideEvents ClientSideEvents { get; set; }
		public JQDropDownList()
		{
			this.ID = "";
			this.Width = 100;
			this.Height = 100;
			this.DropDownWidth = null;
			this.Items = new List<JQListItem>();
			this.TabIndex = 0;
			this.ItemTemplateID = "";
			this.HeaderTemplateID = "";
			this.FooterTemplateID = "";
			this.ToggleImageCssClass = "";
			this.ClientSideEvents = new DropDownListClientSideEvents();
		}
		internal List<Hashtable> SerializeItems(List<JQListItem> items)
		{
			List<Hashtable> list = new List<Hashtable>();
			foreach (JQListItem current in items)
			{
				list.Add(current.ToHashtable());
			}
			return list;
		}
	}
}
