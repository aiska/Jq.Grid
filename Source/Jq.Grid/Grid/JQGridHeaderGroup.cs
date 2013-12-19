using System;
using System.Collections;
namespace Jq.Grid
{
	public class JQGridHeaderGroup
	{
		public string StartColumnName { get; set; }
		public int NumberOfColumns { get; set; }
		public string TitleText { get; set; }
		public JQGridHeaderGroup()
		{
			this.StartColumnName = "";
			this.NumberOfColumns = 1;
			this.TitleText = "";
		}
		internal Hashtable ToHashtable()
		{
			Hashtable hashtable = new Hashtable();
			if (!string.IsNullOrEmpty(this.StartColumnName))
			{
				hashtable["startColumnName"] = this.StartColumnName;
			}
			hashtable["numberOfColumns"] = this.NumberOfColumns;
			if (!string.IsNullOrEmpty(this.TitleText))
			{
				hashtable["titleText"] = this.TitleText;
			}
			return hashtable;
		}
	}
}
