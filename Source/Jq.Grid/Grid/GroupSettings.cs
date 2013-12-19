using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Jq.Grid
{
	public sealed class GroupSettings
	{
		public List<GroupField> GroupFields { get; set; }
		public bool CollapseGroups { get; set; }
		public GroupSettings()
		{
			this.CollapseGroups = false;
			this.GroupFields = new List<GroupField>();
		}
		internal string ToJSON()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.GroupFields.Count > 0)
			{
				stringBuilder.Append(",grouping:true");
				stringBuilder.Append(",groupingView: {");
				stringBuilder.AppendFormat("groupField: {0}", this.GetDataFields());
				stringBuilder.AppendFormat(",groupColumnShow: {0}", this.GetGroupColumnShow());
				stringBuilder.AppendFormat(",groupText: {0}", this.GetHeaderText());
				stringBuilder.AppendFormat(",groupOrder: {0}", this.GetGroupSortDirection());
				stringBuilder.AppendFormat(",groupSummary: {0}", this.GetGroupShowGroupSummary());
				stringBuilder.AppendFormat(",groupCollapse: {0}", this.CollapseGroups.ToString().ToLower());
				stringBuilder.AppendFormat(",groupDataSorted: true", new object[0]);
				stringBuilder.Append("}");
			}
			return stringBuilder.ToString();
		}
		private string GetDataFields()
		{
			List<string> list = new List<string>();
			foreach (GroupField current in this.GroupFields)
			{
				list.Add(current.DataField);
			}
            return JsonConvert.SerializeObject(list);
		}
		private string GetGroupColumnShow()
		{
			List<bool> list = new List<bool>();
			foreach (GroupField current in this.GroupFields)
			{
				list.Add(current.ShowGroupColumn);
			}
            return JsonConvert.SerializeObject(list);
		}
		private string GetHeaderText()
		{
			List<string> list = new List<string>();
			foreach (GroupField current in this.GroupFields)
			{
				list.Add(current.HeaderText);
			}
            return JsonConvert.SerializeObject(list);
		}
		private string GetGroupSortDirection()
		{
			List<string> list = new List<string>();
			foreach (GroupField current in this.GroupFields)
			{
				list.Add(current.GroupSortDirection.ToString().ToLower());
			}
            return JsonConvert.SerializeObject(list);
		}
		private string GetGroupShowGroupSummary()
		{
			List<bool> list = new List<bool>();
			foreach (GroupField current in this.GroupFields)
			{
				list.Add(current.ShowGroupSummary);
			}
			return JsonConvert.SerializeObject(list);
		}
	}
}
