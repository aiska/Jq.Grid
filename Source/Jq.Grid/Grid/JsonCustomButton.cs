using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json;
namespace Jq.Grid
{
	internal class JsonCustomButton
	{
		private Hashtable _jsonValues;
		private JQGridToolBarButton _button;
		public JsonCustomButton(JQGridToolBarButton button)
		{
			this._jsonValues = new Hashtable();
			this._button = button;
		}
		public string Process()
		{
			string value = string.IsNullOrEmpty(this._button.Text) ? " " : this._button.Text;
			if (!string.IsNullOrEmpty(this._button.Text))
			{
				this._jsonValues["caption"] = value;
			}
			if (!string.IsNullOrEmpty(this._button.ButtonIcon))
			{
				this._jsonValues["buttonicon"] = this._button.ButtonIcon;
			}
			this._jsonValues["position"] = this._button.Position.ToString().ToLower();
			if (!string.IsNullOrEmpty(this._button.ToolTip))
			{
				this._jsonValues["title"] = this._button.ToolTip;
			}
            string text = JsonConvert.SerializeObject(this._jsonValues);
			StringBuilder stringBuilder = new StringBuilder();
			this.RenderClientSideEvent(text, stringBuilder, "onClickButton", this._button.OnClick);
			return text.Insert(text.Length - 1, stringBuilder.ToString());
		}
		private void RenderClientSideEvent(string json, StringBuilder sb, string jsName, string eventName)
		{
			if (!string.IsNullOrEmpty(eventName))
			{
				sb.AppendFormat(",{0}:function() {{ {1}(); }}", jsName, eventName);
			}
		}
	}
}
