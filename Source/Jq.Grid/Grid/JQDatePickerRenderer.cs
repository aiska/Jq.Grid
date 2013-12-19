using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
namespace Jq.Grid
{
	internal class JQDatePickerRenderer
	{
		private string _dateSeparator = "";
		private string _dateFormat = "";
		private JQDatePicker _model;
		public JQDatePickerRenderer(JQDatePicker datePicker)
		{
			this._model = datePicker;
			this._dateSeparator = Thread.CurrentThread.CurrentCulture.DateTimeFormat.DateSeparator;
			this._model.DateFormat = datePicker.DateFormat.Replace("/", this._dateSeparator);
			this._dateFormat = "{0:" + datePicker.DateFormat + "}";
		}
		public string RenderHtml()
		{
			if (this._model.DisplayMode == DatePickerDisplayMode.Standalone)
			{
				return this.GetStandaloneJavascript();
			}
			return this.GetControlEditorJavascript();
		}
		private string GetStandaloneJavascript()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input type='text' id='{0}' name='{0}' />", this._model.ID);
			stringBuilder.AppendFormat("<style type='text/css'>body .ui-datepicker { z-index: 2000 }</style>", new object[0]);
			stringBuilder.Append("<script type='text/javascript'>\n");
			stringBuilder.Append("$(document).ready(function() {");
			stringBuilder.AppendFormat("$('#{0}').datepicker({{", this._model.ID);
			stringBuilder.Append(this.GetStartupOptions());
			stringBuilder.Append("});");
			stringBuilder.Append("});");
			stringBuilder.Append("</script>");
			return stringBuilder.ToString();
		}
		private string GetControlEditorJavascript()
		{
			return "<style type='text/css'>body .ui-datepicker { z-index: 2000 }</style>" + string.Format("<script type='text/javascript'>var {0}_dpid = {{ {1} }};</script>", this._model.ID, this.GetStartupOptions());
		}
		private string GetStartupOptions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("id: '{0}'", this._model.ID);
			stringBuilder.AppendFormatIfNotNullOrEmpty(this._model.AltField, ",altField: '{0}'");
			stringBuilder.AppendFormatIfNotNullOrEmpty(this._model.AltFormat, ",altFormat: '{0}'");
			stringBuilder.AppendFormatIfNotNullOrEmpty(this._model.AppendText, ",appendText: '{0}'");
			stringBuilder.AppendIfTrue(this._model.AutoSize, ",autoSize: true");
			stringBuilder.AppendFormatIfNotNullOrEmpty(this._model.ButtonImage, ",buttonImage: '{0}'");
			stringBuilder.AppendIfTrue(this._model.ButtonImageOnly, ",buttonImageOnly: true");
			stringBuilder.AppendFormatIfTrue(this._model.ButtonText != "...", ",buttonText: '{0}'", new object[]
			{
				this._model.ButtonText
			});
			stringBuilder.AppendIfTrue(this._model.ChangeMonth, ",changeMonth: true");
			stringBuilder.AppendIfTrue(this._model.ChangeYear, ",changeYear: true");
			stringBuilder.AppendFormatIfTrue(this._model.CloseText != "Done", ",closeText: '{0}'", new object[]
			{
				this._model.CloseText
			});
			stringBuilder.AppendIfFalse(this._model.ConstrainInput, ",constrainInput: false");
			stringBuilder.AppendFormatIfTrue(this._model.CurrentText != "Today", ",currentText: '{0}'", new object[]
			{
				this._model.CurrentText
			});
			stringBuilder.AppendFormat(",dateFormat: '{0}'", this.ConvertDotNetDateFormatToJQuery(this._model.DateFormat));
			stringBuilder.AppendFormatIfTrue(this._model.DayNames.Count > 0, ",dayNames: {0}", new object[]
			{
                JsonConvert.SerializeObject(this._model.DayNames)
			});
			stringBuilder.AppendFormatIfTrue(this._model.DayNamesMin.Count > 0, ",dayNamesMin: {0}", new object[]
			{
				JsonConvert.SerializeObject(this._model.DayNamesMin)
			});
			stringBuilder.AppendFormatIfTrue(this._model.DayNamesShort.Count > 0, ",dayNamesShort: {0}", new object[]
			{
				JsonConvert.SerializeObject(this._model.DayNamesShort)
			});
			stringBuilder.AppendFormatIfTrue(this._model.DefaultDate.HasValue, ",defaultDate: {0}", new object[]
			{
				this.FormatDateToJavascript(this._model.DefaultDate)
			});
			stringBuilder.AppendFormatIfTrue(this._model.Duration != 500, ",duration: {0}", new object[]
			{
				this._model.Duration.ToString()
			});
			stringBuilder.AppendIfFalse(this._model.Enabled, ",disabled: true");
			stringBuilder.AppendFormatIfTrue(this._model.FirstDay != 0, ",firstDay: {0}", new object[]
			{
				this._model.FirstDay.ToString()
			});
			stringBuilder.AppendIfTrue(this._model.GotoCurrent, ",gotoCurrent: true");
			stringBuilder.AppendIfTrue(this._model.HideIfNoPrevNext, ",hideIfNoPrevNext: true");
			stringBuilder.AppendIfTrue(this._model.IsRTL, "isRTL: true");
			stringBuilder.AppendFormatIfNotNull(this._model.MaxDate, ",minDate: {0}", new object[]
			{
				this.FormatDateToJavascript(this._model.MinDate)
			});
			stringBuilder.AppendFormatIfNotNull(this._model.MinDate, ",maxDate: {0}", new object[]
			{
				this.FormatDateToJavascript(this._model.MaxDate)
			});
			stringBuilder.AppendFormatIfTrue(this._model.MonthNames.Count > 0, ",monthNames: {0}", new object[]
			{
				JsonConvert.SerializeObject(this._model.MonthNames)
			});
			stringBuilder.AppendFormatIfTrue(this._model.MonthNamesShort.Count > 0, ",monthNamesShort: {0}", new object[]
			{
				JsonConvert.SerializeObject(this._model.MonthNamesShort)
			});
			stringBuilder.AppendIfTrue(this._model.NavigationAsDateFormat, ",navigationAsDateFormat: true");
			stringBuilder.AppendFormatIfTrue(this._model.NextText != "Next", ",nextText: '{0}'", new object[]
			{
				this._model.NextText
			});
			stringBuilder.AppendFormatIfTrue(this._model.PrevText != "Prev", ",prevText: '{0}'", new object[]
			{
				this._model.PrevText
			});
			stringBuilder.AppendIfTrue(this._model.ShowButtonPanel, ",showButtonPanel: true");
			stringBuilder.AppendIfTrue(this._model.ShowMonthAfterYear, ",showMonthAfterYear: true");
			stringBuilder.AppendFormat(",showOn: '{0}'", this._model.ShowOn.ToString().ToLower());
			return stringBuilder.ToString();
		}
		private string FormatDateToJavascript(DateTime? dateTime)
		{
			if (dateTime.HasValue)
			{
				return string.Format("new Date({0},{1},{2})", dateTime.Value.Year.ToString(), (dateTime.Value.Month - 1).ToString(), dateTime.Value.Day.ToString());
			}
			return "";
		}
		private string ConvertDotNetDateFormatToJQuery(string dateFormat)
		{
			dateFormat = dateFormat.Replace("yy", "y");
			if (dateFormat.IndexOf("MMMM") > 0)
			{
				dateFormat = dateFormat.Replace("MMMM", "MM");
			}
			else
			{
				if (dateFormat.IndexOf("MMM") > 0)
				{
					dateFormat = dateFormat.Replace("MMM", "M");
				}
				else
				{
					if (dateFormat.IndexOf("MM") > 0)
					{
						dateFormat = dateFormat.Replace("MM", "mm");
					}
					else
					{
						if (dateFormat.IndexOf("M") > 0)
						{
							dateFormat = dateFormat.Replace("M", "n");
						}
					}
				}
			}
			if (dateFormat.IndexOf("DDDD") > 0)
			{
				dateFormat = dateFormat.Replace("DDDD", "DD");
			}
			else
			{
				if (dateFormat.IndexOf("DDD") > 0)
				{
					dateFormat = dateFormat.Replace("DDD", "D");
				}
				else
				{
					if (dateFormat.IndexOf("DD") > 0)
					{
						dateFormat = dateFormat.Replace("DD", "dd");
					}
					else
					{
						if (dateFormat.IndexOf("D") > 0)
						{
							dateFormat = dateFormat.Replace("D", "d");
						}
					}
				}
			}
			return dateFormat;
		}
	}
}
