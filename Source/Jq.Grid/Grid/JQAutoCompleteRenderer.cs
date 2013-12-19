using System;
using System.Text;
using Newtonsoft.Json;
namespace Jq.Grid
{
	internal class JQAutoCompleteRenderer
	{
		private JQAutoComplete _model;
		public JQAutoCompleteRenderer(JQAutoComplete autoComplete)
		{
			this._model = autoComplete;
		}
		public string RenderHtml()
		{
			if (this._model.DisplayMode == AutoCompleteDisplayMode.Standalone)
			{
				return this.GetStandaloneJavascript();
			}
			return this.GetControlEditorJavascript();
		}
		private string GetStandaloneJavascript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input type='text' id='{0}' name='{0}' />", this._model.ID);
			stringBuilder.Append("<script type='text/javascript'>\n");
			stringBuilder.Append("$(document).ready(function() {");
			stringBuilder.AppendFormat("$('#{0}').autocomplete({{", this._model.ID);
			stringBuilder.Append(this.GetStartupOptions());
			stringBuilder.Append("});");
			stringBuilder.Append("});");
			stringBuilder.Append("</script>");
			return stringBuilder.ToString();
		}
		private string GetControlEditorJavascript()
		{
			return string.Format("<script type='text/javascript'>var {0}_acid = {{ {1} }};</script>", this._model.ID, this.GetStartupOptions());
		}
		private string GetStartupOptions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("id: '{0}'", this._model.ID);
			stringBuilder.AppendFormat(",source: '{0}'", this._model.DataUrl);
			stringBuilder.AppendFormatIfTrue(this._model.Delay != 300, ",delay: {0}", new object[]
			{
				this._model.Delay
			});
			stringBuilder.AppendIfFalse(this._model.Enabled, ",disabled: true");
			stringBuilder.AppendFormatIfTrue(this._model.MinLength != 1, ",minLength: {0}", new object[]
			{
				this._model.MinLength
			});
			return stringBuilder.ToString();
		}
	}
}
