using System;
namespace Jq.Grid
{
	public class SearchDialogSettings
	{
		public int TopOffset { get; set; }
		public int LeftOffset { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public bool Modal { get; set; }
		public bool Draggable { get; set; }
		public string FindButtonText { get; set; }
		public string ResetButtonText { get; set; }
		public bool MultipleSearch { get; set; }
		public bool ValidateInput { get; set; }
		public bool Resizable { get; set; }
		public SearchDialogSettings()
		{
			this.TopOffset = (this.LeftOffset = 0);
			this.Width = (this.Height = 300);
			this.Modal = (this.MultipleSearch = (this.ValidateInput = false));
			this.Draggable = true;
			this.FindButtonText = (this.ResetButtonText = "");
		}
	}
}
