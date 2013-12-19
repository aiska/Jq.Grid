using System;
using System.Collections.Generic;
namespace Jq.Grid
{
	public class JQDatePicker
	{
		public bool Enabled { get; set; }
		public string AltField { get; set; }
		public string AltFormat { get; set; }
		public string AppendText { get; set; }
		public bool AutoSize { get; set; }
		public string ButtonImage { get; set; }
		public bool ButtonImageOnly { get; set; }
		public string ButtonText { get; set; }
		public bool ChangeMonth { get; set; }
		public bool ChangeYear { get; set; }
		public string CloseText { get; set; }
		public bool ConstrainInput { get; set; }
		public string CurrentText { get; set; }
		public string DateFormat { get; set; }
		public List<string> DayNames { get; set; }
		public List<string> DayNamesMin { get; set; }
		public List<string> DayNamesShort { get; set; }
		public DatePickerDisplayMode DisplayMode { get; set; }
		public DateTime? DefaultDate { get; set; }
		public int Duration { get; set; }
		public int FirstDay { get; set; }
		public bool GotoCurrent { get; set; }
		public bool HideIfNoPrevNext { get; set; }
		public string ID { get; set; }
		public bool IsRTL { get; set; }
		public DateTime? MaxDate { get; set; }
		public DateTime? MinDate { get; set; }
		public List<string> MonthNames { get; set; }
		public List<string> MonthNamesShort { get; set; }
		public bool NavigationAsDateFormat { get; set; }
		public string NextText { get; set; }
		public string PrevText { get; set; }
		public bool ShowButtonPanel { get; set; }
		public bool ShowMonthAfterYear { get; set; }
		public ShowOn ShowOn { get; set; }
		public JQDatePicker()
		{
			this.Enabled = true;
			this.AltField = "";
			this.AltFormat = "";
			this.AppendText = "";
			this.AutoSize = false;
			this.ButtonImage = "";
			this.ButtonImageOnly = false;
			this.ButtonText = "...";
			this.ChangeMonth = false;
			this.ChangeYear = false;
			this.CloseText = "Done";
			this.ConstrainInput = true;
			this.CurrentText = "Today";
			this.DateFormat = "yyyy/MM/dd";
			this.DayNames = new List<string>();
			this.DayNamesMin = new List<string>();
			this.DayNamesShort = new List<string>();
			this.DisplayMode = DatePickerDisplayMode.Standalone;
			this.DefaultDate = null;
			this.Duration = 500;
			this.FirstDay = 0;
			this.GotoCurrent = false;
			this.HideIfNoPrevNext = false;
			this.IsRTL = false;
			this.MaxDate = null;
			this.MinDate = null;
			this.MonthNames = new List<string>();
			this.MonthNamesShort = new List<string>();
			this.NavigationAsDateFormat = false;
			this.NextText = "Next";
			this.PrevText = "Prev";
			this.ShowButtonPanel = false;
			this.ShowMonthAfterYear = false;
			this.ShowOn = ShowOn.Both;
		}
	}
}
