using Jq.Grid;
using System;
using System.ComponentModel;
using System.Web.Mvc;
namespace Jq.Grid
{
	public class JqueryGridNamespace
	{
        public MvcHtmlString JQGridPartial(JQGrid grid)
        {
            //JQGridRenderer jQGridRenderer = new JQGridRenderer();
            return MvcHtmlString.Create(JQGridRenderer.RenderHtml(grid, JQGridRenderMode.Partial));
        }
        public MvcHtmlString JQGridScript(JQGrid grid)
        {
            //JQGridRenderer jQGridRenderer = new JQGridRenderer();
            return MvcHtmlString.Create(JQGridRenderer.RenderHtml(grid, JQGridRenderMode.Script));
        }
        public MvcHtmlString JQGrid(JQGrid grid)
        {
            //JQGridRenderer jQGridRenderer = new JQGridRenderer();
            return MvcHtmlString.Create(JQGridRenderer.RenderHtml(grid, JQGridRenderMode.Full));
        }
        public MvcHtmlString JQGrid(JQGrid grid, string id)
        {
            //JQGridRenderer jQGridRenderer = new JQGridRenderer();
            grid.ID = id;
            return MvcHtmlString.Create(JQGridRenderer.RenderHtml(grid, JQGridRenderMode.Full));
        }
        public MvcHtmlString JQTreeView(JQTreeView tree, string id)
		{
			JQTreeViewRenderer jQTreeViewRenderer = new JQTreeViewRenderer(tree);
			tree.ID = id;
			return MvcHtmlString.Create(jQTreeViewRenderer.RenderHtml());
		}
		public MvcHtmlString JQDropDownList(JQDropDownList dropDownList, string id)
		{
			JQDropDownListRenderer jQDropDownListRenderer = new JQDropDownListRenderer(dropDownList);
			dropDownList.ID = id;
			return MvcHtmlString.Create(jQDropDownListRenderer.RenderHtml());
		}
		public MvcHtmlString JQMultiSelect(JQMultiSelect multiSelect, string id)
		{
			JQMultiSelectRenderer jQMultiSelectRenderer = new JQMultiSelectRenderer(multiSelect);
			multiSelect.ID = id;
			return MvcHtmlString.Create(jQMultiSelectRenderer.RenderHtml());
		}
		public MvcHtmlString JQDatePicker(JQDatePicker datePicker, string id)
		{
			JQDatePickerRenderer jQDatePickerRenderer = new JQDatePickerRenderer(datePicker);
			datePicker.ID = id;
			return MvcHtmlString.Create(jQDatePickerRenderer.RenderHtml());
		}
		public MvcHtmlString JQAutoComplete(JQAutoComplete autoComplete, string id)
		{
			JQAutoCompleteRenderer jQAutoCompleteRenderer = new JQAutoCompleteRenderer(autoComplete);
			autoComplete.ID = id;
			return MvcHtmlString.Create(jQAutoCompleteRenderer.RenderHtml());
		}
        //public MvcHtmlString Chart(Chart chart, string id)
        //{
        //    ChartRenderer ChartRenderer = new ChartRenderer(chart);
        //    chart.ID = id;
        //    return MvcHtmlString.Create(ChartRenderer.RenderHtml());
        //}
		[EditorBrowsable(EditorBrowsableState.Never)]
		private new bool Equals(object value)
		{
			return base.Equals(value);
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		private new int GetHashCode()
		{
			return base.GetHashCode();
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		private new Type GetType()
		{
			return base.GetType();
		}
		[EditorBrowsable(EditorBrowsableState.Never)]
		private new string ToString()
		{
			return base.ToString();
		}
	}
}
