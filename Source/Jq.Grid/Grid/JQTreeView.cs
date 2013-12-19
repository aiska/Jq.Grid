using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
namespace Jq.Grid
{
	public class JQTreeView
	{
		public string ID { get; set; }
		public string DataUrl { get; set; }
		public string DragAndDropUrl { get; set; }
		public Unit Height { get; set; }
		public Unit Width { get; set; }
		public bool HoverOnMouseOver { get; set; }
		public bool CheckBoxes { get; set; }
		public bool MultipleSelect { get; set; }
		public TreeViewClientSideEvents ClientSideEvents { get; set; }
		public string NodeTemplateID { get; set; }
		public bool DragAndDrop { get; set; }
		public JQTreeView()
		{
			this.ID = "";
			this.DataUrl = "";
			this.DragAndDropUrl = "";
			this.Width = Unit.Empty;
			this.Height = Unit.Empty;
			this.HoverOnMouseOver = true;
			this.CheckBoxes = false;
			this.MultipleSelect = false;
			this.NodeTemplateID = "";
			this.ClientSideEvents = new TreeViewClientSideEvents();
			this.DragAndDrop = false;
		}

        [HttpPost]
		public JsonResult DataBind(List<JQTreeNode> nodes)
		{
			JsonResult jsonResult = new JsonResult();
            jsonResult.JsonRequestBehavior = JsonRequestBehavior.DenyGet;
            jsonResult.Data = JsonConvert.SerializeObject(this.SerializeNodes(nodes));
            return jsonResult;
		}
		private List<Hashtable> SerializeNodes(List<JQTreeNode> nodes)
		{
			List<Hashtable> list = new List<Hashtable>();
			foreach (JQTreeNode current in nodes)
			{
				list.Add(current.ToHashtable());
			}
			return list;
		}
		public List<JQTreeNode> GetAllNodesFlat(List<JQTreeNode> nodes)
		{
			List<JQTreeNode> list = new List<JQTreeNode>();
			foreach (JQTreeNode current in nodes)
			{
				list.Add(current);
				if (current.Nodes.Count > 0)
				{
					this.GetNodesFlat(current.Nodes, list);
				}
			}
			return list;
		}
		private void GetNodesFlat(List<JQTreeNode> nodes, List<JQTreeNode> result)
		{
			foreach (JQTreeNode current in nodes)
			{
				result.Add(current);
				if (current.Nodes.Count > 0)
				{
					this.GetNodesFlat(current.Nodes, result);
				}
			}
		}
		public JQTreeNodeDropEventArgs GetDragDropInfo()
		{
			JQTreeNodeDropEventArgs result = new JQTreeNodeDropEventArgs();
			NameValueCollection arg_15_0 = HttpContext.Current.Request.Form;
			return result;
		}
	}
}
