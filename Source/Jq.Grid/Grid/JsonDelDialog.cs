using System;
using System.Collections;
using Newtonsoft.Json;
namespace Jq.Grid
{
	internal class JsonDelDialog
	{
		private Hashtable _jsonValues;
		private JQGrid _grid;
		public JsonDelDialog(JQGrid grid)
		{
			this._jsonValues = new Hashtable();
			this._grid = grid;
		}
		public string Process()
		{
			DeleteDialogSettings deleteDialogSettings = this._grid.DeleteDialogSettings;
			if (deleteDialogSettings.TopOffset != 0)
			{
				this._jsonValues["top"] = deleteDialogSettings.TopOffset;
			}
			if (deleteDialogSettings.LeftOffset != 0)
			{
				this._jsonValues["left"] = deleteDialogSettings.LeftOffset;
			}
			if (deleteDialogSettings.Width != 300)
			{
				this._jsonValues["width"] = deleteDialogSettings.Width;
			}
			if (deleteDialogSettings.Height != 300)
			{
				this._jsonValues["height"] = deleteDialogSettings.Height;
			}
			if (deleteDialogSettings.Modal)
			{
				this._jsonValues["modal"] = true;
			}
			if (!deleteDialogSettings.Draggable)
			{
				this._jsonValues["drag"] = false;
			}
			if (!string.IsNullOrEmpty(deleteDialogSettings.SubmitText))
			{
				this._jsonValues["bSubmit"] = deleteDialogSettings.SubmitText;
			}
			if (!string.IsNullOrEmpty(deleteDialogSettings.CancelText))
			{
				this._jsonValues["bCancel"] = deleteDialogSettings.CancelText;
			}
			if (!string.IsNullOrEmpty(deleteDialogSettings.LoadingMessageText))
			{
				this._jsonValues["processData"] = deleteDialogSettings.LoadingMessageText;
			}
			if (!string.IsNullOrEmpty(deleteDialogSettings.Caption))
			{
				this._jsonValues["caption"] = deleteDialogSettings.Caption;
			}
			if (!string.IsNullOrEmpty(deleteDialogSettings.DeleteMessage))
			{
				this._jsonValues["msg"] = deleteDialogSettings.DeleteMessage;
			}
			if (!deleteDialogSettings.ReloadAfterSubmit)
			{
				this._jsonValues["reloadAfterSubmit"] = false;
			}
			if (!deleteDialogSettings.Resizable)
			{
				this._jsonValues["resize"] = false;
			}
			this._jsonValues["recreateForm"] = true;
            string json = JsonConvert.SerializeObject(this._jsonValues);
			ClientSideEvents clientSideEvents = this._grid.ClientSideEvents;
			json = JsonUtil.RenderClientSideEvent(json, "beforeShowForm", clientSideEvents.BeforeDeleteDialogShown);
			json = JsonUtil.RenderClientSideEvent(json, "afterShowForm", clientSideEvents.AfterDeleteDialogShown);
			json = JsonUtil.RenderClientSideEvent(json, "afterComplete", clientSideEvents.AfterDeleteDialogRowDeleted);
			json = JsonUtil.RenderClientSideEvent(json, "errorTextFormat", "function(data) { return 'Error: ' + data.responseText }");
			return JsonUtil.RenderClientSideEvent(json, "delData", string.Format("{{ __RequestVerificationToken: $('input[name=__RequestVerificationToken]').val() }}", this._grid.ID));
		}
	}
}
