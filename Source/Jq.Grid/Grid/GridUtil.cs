using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
namespace Jq.Grid
{
	internal static class GridUtil
	{
		internal static List<string> GetListOfColumns(JQGrid grid)
		{
			List<string> result = new List<string>();
			grid.Columns.FindAll((JQGridColumn c) => c.EditType == EditType.DatePicker || c.EditType == EditType.AutoComplete).ForEach(delegate(JQGridColumn c)
			{
				result.Add(c.EditType.ToString().ToLower() + ":" + c.DataField);
			});
			return result;
		}
		internal static List<string> GetListOfEditors(JQGrid grid)
		{
			List<string> result = new List<string>();
			grid.Columns.FindAll((JQGridColumn c) => c.EditType == EditType.DatePicker || c.EditType == EditType.AutoComplete).ForEach(delegate(JQGridColumn c)
			{
				result.Add(c.EditorControlID);
			}
			);
			return result;
		}
		internal static List<string> GetListOfSearchColumns(JQGrid grid)
		{
			List<string> result = new List<string>();
			grid.Columns.FindAll((JQGridColumn c) => c.SearchType == SearchType.DatePicker || c.SearchType == SearchType.AutoComplete).ForEach(delegate(JQGridColumn c)
			{
				result.Add(c.SearchType.ToString().ToLower() + ":" + c.DataField);
			}
			);
			return result;
		}
		internal static List<string> GetListOfSearchEditors(JQGrid grid)
		{
			List<string> result = new List<string>();
			grid.Columns.FindAll((JQGridColumn c) => c.SearchType == SearchType.DatePicker || c.SearchType == SearchType.AutoComplete).ForEach(delegate(JQGridColumn c)
			{
				result.Add(c.SearchControlID);
			}
			);
			return result;
		}
		internal static string GetAttachEditorsFunction(JQGrid grid, string editorType, string editorControlID)
		{
			GridUtil.GetListOfColumns(grid);
			GridUtil.GetListOfEditors(grid);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("function(el) {");
			stringBuilder.Append("setTimeout(function() {");
			stringBuilder.AppendFormat("var ec = '{0}';", editorControlID);
			if (editorType == "datepicker")
			{
				stringBuilder.Append("if (typeof($(el).datepicker) !== 'function')");
				stringBuilder.Append("alert('JQDatePicker javascript not present on the page. Please, include jquery.jqDatePicker.min.js');");
				stringBuilder.Append("$(el).datepicker( eval(ec + '_dpid') );");
			}
			if (editorType == "autocomplete")
			{
				stringBuilder.Append("if (typeof($(el).autocomplete) !== 'function')");
				stringBuilder.Append("alert('JQAutoComplete javascript not present on the page. Please, include jquery.jqAutoComplete.min.js');");
				stringBuilder.Append("$(el).autocomplete( eval(ec + '_acid') );");
			}
			stringBuilder.Append("},200);");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}
	}
}
