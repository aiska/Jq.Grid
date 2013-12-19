using System;
using System.Collections;
using System.Collections.Specialized;
using Newtonsoft.Json;
namespace Jq.Grid
{
	public class JQListItem
	{
		public string Text { get; set; }
		public string Value { get; set; }
		public bool Enabled { get; set; }
		public bool Selected { get; set; }
		public string Url { get; set; }
		public string ImageUrl { get; set; }
		public string ExpandedImageUrl { get; set; }
		public NameValueCollection Options { get; set; }
		public string ItemTemplateID { get; set; }
		public JQListItem()
		{
			this.Text = "";
			this.Value = "";
			this.Selected = false;
			this.Enabled = true;
			this.Url = "";
			this.ImageUrl = "";
			this.Options = new NameValueCollection();
			this.ItemTemplateID = "";
		}
		public string ToJSON()
		{
			return JsonConvert.SerializeObject(this.ToHashtable());
		}
		internal Hashtable ToHashtable()
		{
			Hashtable hashtable = new Hashtable();
			if (!string.IsNullOrEmpty(this.Text))
			{
				hashtable.Add("text", this.Text);
			}
			if (!string.IsNullOrEmpty(this.Value))
			{
				hashtable.Add("value", this.Value);
			}
			if (!this.Enabled)
			{
				hashtable.Add("enabled", false);
			}
			if (this.Selected)
			{
				hashtable.Add("selected", true);
			}
			if (!string.IsNullOrEmpty(this.Url))
			{
				hashtable.Add("url", this.Url);
			}
			if (!string.IsNullOrEmpty(this.ImageUrl))
			{
				hashtable.Add("imageUrl", this.ImageUrl);
			}
			if (!string.IsNullOrEmpty(this.ExpandedImageUrl))
			{
				hashtable.Add("expandedImageUrl", this.ExpandedImageUrl);
			}
			if (!string.IsNullOrEmpty(this.ItemTemplateID))
			{
				hashtable.Add("itemTemplateID", this.ItemTemplateID);
			}
			string[] allKeys = this.Options.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				string text = allKeys[i];
				string text2 = this.Options[text];
				if (text2 != null)
				{
					hashtable.Add(text, text2);
				}
			}
			return hashtable;
		}
	}
}
