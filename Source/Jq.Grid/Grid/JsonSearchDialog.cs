using Newtonsoft.Json;
using System;
using System.Collections;
namespace Jq.Grid
{
	internal class JsonSearchDialog
	{
		private Hashtable _jsonValues;
		private JQGrid _grid;
		public Hashtable JsonValues
		{
			get
			{
				return this._jsonValues;
			}
			set
			{
				this._jsonValues = value;
			}
		}
		public JsonSearchDialog(JQGrid grid)
		{
			this._jsonValues = new Hashtable();
			this._grid = grid;
		}
		public string Process()
		{
			SearchDialogSettings searchDialogSettings = this._grid.SearchDialogSettings;
			if (searchDialogSettings.TopOffset != 0)
			{
				this._jsonValues["top"] = searchDialogSettings.TopOffset;
			}
			if (searchDialogSettings.LeftOffset != 0)
			{
				this._jsonValues["left"] = searchDialogSettings.LeftOffset;
			}
			if (searchDialogSettings.Width != 300)
			{
				this._jsonValues["width"] = searchDialogSettings.Width;
			}
			if (searchDialogSettings.Height != 300)
			{
				this._jsonValues["height"] = searchDialogSettings.Height;
			}
			if (searchDialogSettings.Modal)
			{
				this._jsonValues["modal"] = true;
			}
			if (!searchDialogSettings.Draggable)
			{
				this._jsonValues["drag"] = false;
			}
			if (!string.IsNullOrEmpty(searchDialogSettings.FindButtonText))
			{
				this._jsonValues["Find"] = searchDialogSettings.FindButtonText;
			}
			if (!string.IsNullOrEmpty(searchDialogSettings.ResetButtonText))
			{
				this._jsonValues["Clear"] = searchDialogSettings.ResetButtonText;
			}
			if (searchDialogSettings.MultipleSearch)
			{
				this._jsonValues["multipleSearch"] = true;
			}
			if (searchDialogSettings.ValidateInput)
			{
				this._jsonValues["checkInput"] = true;
			}
			if (!searchDialogSettings.Resizable)
			{
				this._jsonValues["resize"] = false;
			}
			this._jsonValues["recreateForm"] = true;
			return JsonConvert.SerializeObject(this._jsonValues);
		}
	}
}
