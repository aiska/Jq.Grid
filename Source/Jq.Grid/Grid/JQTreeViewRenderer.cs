using System;
using System.Text;
namespace Jq.Grid
{
	internal class JQTreeViewRenderer
	{
		private JQTreeView _model;
		public JQTreeViewRenderer(JQTreeView model)
		{
			this._model = model;
		}
		public string RenderHtml()
		{
			return this.GetStandaloneJavascript();
		}
		private string GetStandaloneJavascript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<div id='{0}_wrapper' class='ui-widget ui-jqtreeview-wrapper' style='width:{1}; height:{2};'>", this._model.ID, this._model.Width.ToString(), this._model.Height.ToString());
			stringBuilder.AppendFormat("<ul id='{0}'></ul>", this._model.ID);
			stringBuilder.Append("</div>");
			stringBuilder.Append("<script type='text/javascript'>\n");
			stringBuilder.Append("$(document).ready(function() {");
			stringBuilder.AppendFormat("$('#{0}').jqTreeView({{", this._model.ID);
			stringBuilder.Append(this.GetStartupOptions());
			stringBuilder.Append("});");
			stringBuilder.Append("});");
			stringBuilder.Append("</script>");
			return stringBuilder.ToString();
		}
		private string GetStartupOptions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			JQTreeView model = this._model;
			TreeViewClientSideEvents clientSideEvents = model.ClientSideEvents;
			stringBuilder.AppendFormat("id: '{0}'", model.ID);
			if (!string.IsNullOrEmpty(model.DataUrl))
			{
				stringBuilder.AppendFormat(",dataUrl: '{0}'", model.DataUrl);
			}
			if (!string.IsNullOrEmpty(model.DragAndDropUrl))
			{
				stringBuilder.AppendFormat(",dragAndDropUrl: '{0}'", model.DragAndDropUrl);
			}
			if (!model.HoverOnMouseOver)
			{
				stringBuilder.AppendFormat(",hoverOnMouseOver:false", new object[0]);
			}
			if (model.CheckBoxes)
			{
				stringBuilder.Append(",checkBoxes:true");
			}
			if (model.MultipleSelect)
			{
				stringBuilder.Append(",multipleSelect:true");
			}
			if (model.DragAndDrop)
			{
				stringBuilder.Append(",dragAndDrop:true");
			}
			if (!string.IsNullOrEmpty(model.NodeTemplateID))
			{
				stringBuilder.AppendFormat(",nodeTemplateID:'{0}'", model.NodeTemplateID);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.Check))
			{
				stringBuilder.AppendFormat(",onCheck:{0}", clientSideEvents.Check);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.Collapse))
			{
				stringBuilder.AppendFormat(",onCollapse:{0}", clientSideEvents.Collapse);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.Expand))
			{
				stringBuilder.AppendFormat(",onExpand:{0}", clientSideEvents.Expand);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.MouseOut))
			{
				stringBuilder.AppendFormat(",onMouseOut:{0}", clientSideEvents.MouseOut);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.MouseOver))
			{
				stringBuilder.AppendFormat(",onMouseOver:{0}", clientSideEvents.MouseOver);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.Select))
			{
				stringBuilder.AppendFormat(",onSelect:{0}", clientSideEvents.Select);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.NodesDragged))
			{
				stringBuilder.AppendFormat(",onNodesDragged:{0}", clientSideEvents.NodesDragged);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.NodesMoved))
			{
				stringBuilder.AppendFormat(",onNodesMoved:{0}", clientSideEvents.NodesMoved);
			}
			if (!string.IsNullOrEmpty(clientSideEvents.NodesDropped))
			{
				stringBuilder.AppendFormat(",onNodesDropped:{0}", clientSideEvents.NodesDropped);
			}
			return stringBuilder.ToString();
		}
	}
}
