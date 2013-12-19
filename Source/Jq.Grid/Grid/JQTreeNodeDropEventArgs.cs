using System;
using System.Collections.Generic;
namespace Jq.Grid
{
	public class JQTreeNodeDropEventArgs
	{
		public List<JQTreeNode> DraggedNodes { get; set; }
		public JQTreeNode DestinationNode { get; set; }
		public string SourceTreeViewID { get; set; }
		public JQTreeNodeDropEventArgs()
		{
		}
		public JQTreeNodeDropEventArgs(List<JQTreeNode> draggedNodes, JQTreeNode destinationNode, string sourceTreeViewID)
		{
			this.DraggedNodes = draggedNodes;
			this.DestinationNode = destinationNode;
			this.SourceTreeViewID = this.SourceTreeViewID;
		}
	}
}
