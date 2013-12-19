using System;
namespace Jq.Grid
{
	public class EditActionIconsSettings
	{
		public bool ShowEditIcon { get; set; }
		public bool ShowDeleteIcon { get; set; }
		public bool SaveOnEnterKeyPress { get; set; }
		public EditActionIconsSettings()
		{
			this.ShowEditIcon = true;
			this.ShowDeleteIcon = true;
			this.SaveOnEnterKeyPress = false;
		}
	}
}
